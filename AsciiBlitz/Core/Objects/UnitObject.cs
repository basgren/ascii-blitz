using AsciiBlitz.Core.Types;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Objects;

public abstract class UnitObject() : GameObject() {
  public Vec2 Pos;
  public Direction Dir = Direction.Down;

  public virtual void Update(float deltaTime) {
  }
}
