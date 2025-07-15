namespace AsciiBlitz.Core.Input;

public interface IGameInput {
  public event Action MoveLeft;
  public event Action MoveRight;
  public event Action MoveUp;
  public event Action MoveDown;
  public event Action Fire;
  public event Action Quit;

  public void Update();
}
