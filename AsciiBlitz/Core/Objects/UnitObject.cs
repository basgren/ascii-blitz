using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Objects;

public abstract class UnitObject : GameObject {
  public Vec2 Pos;
  public Direction Dir = Direction.Down;
  private IGameState? _gameState;

  public IGameState GameState {
    get => _gameState ?? throw new NullReferenceException($"Game state is not set for object {this}");
    set => _gameState = value;
  }

  public virtual void Update(float deltaTime) {
  }
}
