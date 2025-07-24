using System.Runtime.InteropServices;
using OpenTK.Audio.OpenAL;

namespace AsciiBlitz.Core.Audio;

public class GameSoundService : IDisposable
{
  public static GameSoundService Instance { get; } = new();

  private readonly string _soundDirectory;
  private readonly Dictionary<string, int> _buffers = new();
  private readonly List<int> _sources = new();

  private ALDevice _device;
  private ALContext _context;

  public GameSoundService()
  {
    _soundDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds");

    _device = ALC.OpenDevice(null);
    if (_device == ALDevice.Null)
      throw new Exception("Failed to open audio device");

    _context = ALC.CreateContext(_device, (int[])null);
    if (_context == ALContext.Null)
      throw new Exception("Failed to create OpenAL context");

    ALC.MakeContextCurrent(_context);
  }

  public void PlaySound(string fileName)
  {
    string path = Path.Combine(_soundDirectory, fileName);
    if (!File.Exists(path))
    {
      Console.WriteLine($"[SoundService] File not found: {path}");
      return;
    }

    if (!_buffers.TryGetValue(fileName, out int buffer))
    {
      var (pcm, format, rate) = LoadWave(path);
      buffer = AL.GenBuffer();

      GCHandle handle = GCHandle.Alloc(pcm, GCHandleType.Pinned);
      IntPtr ptr = handle.AddrOfPinnedObject();
      AL.BufferData(buffer, format, ptr, pcm.Length, rate);
      handle.Free();

      _buffers[fileName] = buffer;
    }

    int source = AL.GenSource();
    AL.Source(source, ALSourcei.Buffer, buffer);
    AL.SourcePlay(source);

    _sources.Add(source);
  }

  public void StopAll()
  {
    foreach (int source in _sources)
    {
      AL.SourceStop(source);
      AL.DeleteSource(source);
    }
    _sources.Clear();
  }

  public void Dispose()
  {
    StopAll();

    foreach (var buffer in _buffers.Values)
    {
      AL.DeleteBuffer(buffer);
    }
    _buffers.Clear();

    ALC.MakeContextCurrent(ALContext.Null);
    if (_context != ALContext.Null)
    {
      ALC.DestroyContext(_context);
      _context = ALContext.Null;
    }

    if (_device != ALDevice.Null)
    {
      ALC.CloseDevice(_device);
      _device = ALDevice.Null;
    }
  }

  private static (byte[] data, ALFormat format, int sampleRate) LoadWave(string filePath)
  {
    using var stream = File.OpenRead(filePath);
    using var reader = new BinaryReader(stream);

    string riff = new string(reader.ReadChars(4));
    if (riff != "RIFF") throw new InvalidDataException("Not a WAV file (no RIFF)");

    reader.ReadInt32(); // file size
    string wave = new string(reader.ReadChars(4));
    if (wave != "WAVE") throw new InvalidDataException("Not a WAV file (no WAVE)");

    ALFormat format = ALFormat.Mono16;
    int sampleRate = 44100;
    byte[]? soundData = null;

    while (reader.BaseStream.Position < reader.BaseStream.Length)
    {
      string chunkId = new string(reader.ReadChars(4));
      int chunkSize = reader.ReadInt32();

      if (chunkId == "fmt ")
      {
        short audioFormat = reader.ReadInt16();
        short numChannels = reader.ReadInt16();
        sampleRate = reader.ReadInt32();
        reader.ReadInt32(); // byte rate
        reader.ReadInt16(); // block align
        short bitsPerSample = reader.ReadInt16();

        if (audioFormat != 1)
          throw new NotSupportedException("Only PCM format is supported");

        format = (numChannels, bitsPerSample) switch
        {
          (1, 8) => ALFormat.Mono8,
          (1, 16) => ALFormat.Mono16,
          (2, 8) => ALFormat.Stereo8,
          (2, 16) => ALFormat.Stereo16,
          _ => throw new NotSupportedException($"Unsupported WAV format: {numChannels}ch, {bitsPerSample}bit")
        };

        int extra = chunkSize - 16;
        if (extra > 0) reader.ReadBytes(extra);
      }
      else if (chunkId == "data")
      {
        soundData = reader.ReadBytes(chunkSize);
      }
      else
      {
        reader.ReadBytes(chunkSize);
      }
    }

    if (soundData == null)
      throw new InvalidDataException("No data chunk found in WAV");

    return (soundData, format, sampleRate);
  }
}