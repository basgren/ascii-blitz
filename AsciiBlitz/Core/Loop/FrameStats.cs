using System.Diagnostics;

namespace AsciiBlitz;

public interface IFrameContext {
  float FrameProcessingTime { get; }
  float DeltaTime { get; }
  float ElapsedTime { get; }
  int FrameIndex { get; }
  float AverageDeltaTime { get; }
  float FPS { get; }
}

public class FrameStats : IFrameContext {
  private readonly float[] _frameTimes;
  private int _index;
  private int _count;
  private float _total;

  private readonly Stopwatch _stopwatch = new();

  private float _deltaTime;
  private float _elapsedTime;
  private int _frameIndex;
  private float _remainingFrameTime;

  private float _targetDelta;
  private float _frameStartTime;
  private float _frameProcessingTime;

  public float DeltaTime => _deltaTime;
  public float ElapsedTime => _elapsedTime;
  public int FrameIndex => _frameIndex;
  public float AverageDeltaTime => _count > 0 ? _total / _count : 0f;
  public float FPS => AverageDeltaTime > 0f ? 1f / AverageDeltaTime : 0f;
  
  /// <summary>
  /// Execution time of frame update function in seconds.
  /// </summary>
  public float FrameProcessingTime => _frameProcessingTime;
  
  /// <summary>
  /// Remaining frame time - how much time in seconds is left until next frame after executing
  /// frame update function. In general DeltaTime = FrameProcessingTime + RemainingFrameTime
  /// </summary>
  public float RemainingFrameTime => _remainingFrameTime;

  public FrameStats(float targetFps = 60f, int maxSamples = 60) {
    _frameTimes = new float[maxSamples];
    _targetDelta = 1f / targetFps;
    _stopwatch.Start();
  }

  /// <summary>
  /// Must be called at the very beginning of game loop, before frame processing function is called.
  /// </summary>
  public void FrameUpdateStarted() {
    _frameIndex++;
    _frameStartTime = (float)_stopwatch.Elapsed.TotalSeconds;
  }

  /// <summary>
  /// Must be called right after frame processing function is called. This will recalculate frame processing time
  /// and remaining frame time = [target frame time] - [frame processing time]
  /// </summary>
  public void FrameUpdateFinished() {
    float now = (float)_stopwatch.Elapsed.TotalSeconds;

    _deltaTime = now - _elapsedTime;
    _elapsedTime = now;
    _frameProcessingTime = now - _frameStartTime;

    // Update rolling average
    _total -= _frameTimes[_index];
    _frameTimes[_index] = _deltaTime;
    _total += _deltaTime;

    _index = (_index + 1) % _frameTimes.Length;
    _count = Math.Min(_count + 1, _frameTimes.Length);
    
    _remainingFrameTime = _targetDelta - _frameProcessingTime;
  }
}
