namespace AsciiBlitz.Core.Input;

public class ConsoleInput: IGameInput {
  private ConsoleKeyInfo? _pressedKey;
  
  /// <summary>
  /// Returns key, captured by the last call of `Update()` method, or null if no key was pressed.  
  /// </summary>
  /// <returns></returns>
  public ConsoleKey? GetKey() {
    return _pressedKey?.Key;
  }
  
  /// <summary>
  /// Update must be called only once in each game loop iteration.
  /// </summary>
  public void Update() {
    _pressedKey = null;

    if (Console.KeyAvailable) {
      _pressedKey = Console.ReadKey(true);

      // Additionally, exhaust buffer, as if we hold the key, it will do auto repeat and accumulate
      // keypresses, and consequent calls of ReadKey will give us the same key for some time even
      // if we've already released the button.
      while (Console.KeyAvailable) {
        Console.ReadKey(true);
      }
    }
  }
}
