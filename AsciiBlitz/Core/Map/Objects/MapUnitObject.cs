using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public abstract class MapUnitObject() : GameObject(), IDestroyable {
  public Vec2 Pos;
  public Direction Dir = Direction.Down;
  public event Action<IDestroyable>? OnDestroyed;
  public void Destroy() => OnDestroyed?.Invoke(this);

  public virtual void Update(float deltaTime) {
  }
}
