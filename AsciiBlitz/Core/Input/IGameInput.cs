namespace AsciiBlitz.Core.Input;

public interface IGameInput {
  void Update(); // Not sure if it should be exposed via interface.
  ConsoleKey? GetKey();
  void Consume();
}
