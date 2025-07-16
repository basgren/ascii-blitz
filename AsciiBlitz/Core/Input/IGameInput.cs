namespace AsciiBlitz.Core.Input;

public interface IGameInput {
  ConsoleKey? GetKey();
  void Consume();
}
