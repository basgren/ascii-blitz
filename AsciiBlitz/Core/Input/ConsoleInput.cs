namespace AsciiBlitz.Core.Input;

public class ConsoleInput : IGameInput {
  public event Action? MoveLeft;
  public event Action? MoveRight;
  public event Action? MoveUp;
  public event Action? MoveDown;
  public event Action? Fire;
  public event Action? Quit;

  public void Update() {
    ConsoleKeyInfo? keyInfo = null;

    if (Console.KeyAvailable) {
      keyInfo = Console.ReadKey(true);
      
      // Also exhaust buffer, as it we hold key, it will accumulate keypresses, and consequent calls of
      // ReadKey will give us the same key for some time even if we've already released the button.
      while (Console.KeyAvailable) {
        Console.ReadKey(true);
      }
    }

    if (!keyInfo.HasValue) {
      return;
    }

    switch (keyInfo.Value.Key) {
      case ConsoleKey.Escape:
        Quit?.Invoke();
        break;

      case ConsoleKey.UpArrow:
        MoveUp?.Invoke();
        break;

      case ConsoleKey.DownArrow:
        MoveDown?.Invoke();
        break;

      case ConsoleKey.LeftArrow:
        MoveLeft?.Invoke();
        break;

      case ConsoleKey.RightArrow:
        MoveRight?.Invoke();
        break;

      case ConsoleKey.Spacebar:
        Fire?.Invoke();
        break;
    }
  }
}
