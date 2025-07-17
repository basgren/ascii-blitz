using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Objects;

public abstract class UnitObject : GameObject {
  public Vec2 Pos;
  public Vec2 Size { get; protected set; } = new(1f, 1f); // 1x1 grid cell.
  public Direction Dir = Direction.Right;
  private IGameState? _gameState;

  public IGameState GameState {
    get => _gameState ?? throw new NullReferenceException($"Game state is not set for object {this}");
    set => _gameState = value;
  }

  public virtual void Update(float deltaTime) {
  }
}
