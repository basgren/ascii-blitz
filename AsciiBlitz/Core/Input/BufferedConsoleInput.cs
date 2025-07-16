namespace AsciiBlitz.Core.Input;

/// <summary>
/// Console input handler with simple key buffering.
/// It captures a key press and keeps it available for a short time
/// to make controls more responsive.
/// </summary>
public class BufferedConsoleInput : IGameInput {
  private static readonly TimeSpan BufferDuration = TimeSpan.FromSeconds(0.5);

  private ConsoleKey? _bufferedKey;
  private DateTime _bufferExpiresAt = DateTime.MinValue;

  /// <summary>
  /// Returns the currently buffered key, or null if no key is buffered
  /// or the buffer duration has expired.
  /// </summary>
  public ConsoleKey? GetKey() {
    if (_bufferedKey != null && DateTime.UtcNow < _bufferExpiresAt) {
      return _bufferedKey;
    }

    Clear();
    return null;
  }

  /// <summary>
  /// Marks the current buffered key as consumed.
  /// After this call, GetKey() will return null until a new key is pressed.
  /// Must be called after key is handled in consumer.
  /// </summary>
  public void Consume() {
    Clear();
  }

  /// <summary>
  /// Immediately clears the buffered key and resets the buffer timer.
  /// </summary>
  private void Clear() {
    _bufferedKey = null;
    _bufferExpiresAt = DateTime.MinValue;
  }

  /// <summary>
  /// Captures the current key press, if available, and updates the internal buffer.
  /// Must be called once per game loop iteration.
  /// </summary>
  public void Update() {
    if (!Console.KeyAvailable) {
      return;
    }

    var keyInfo = Console.ReadKey(intercept: true);
      
    _bufferedKey = keyInfo.Key;
    _bufferExpiresAt = DateTime.UtcNow + BufferDuration;

    // Discard any repeated keypresses due to key repeat behavior
    while (Console.KeyAvailable) {
      Console.ReadKey(intercept: true);
    }
  }
}
