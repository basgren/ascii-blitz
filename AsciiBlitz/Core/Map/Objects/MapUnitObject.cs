using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public abstract class MapUnitObject() : GameObject() {
  public Vec2 Pos;
  public Direction Dir = Direction.Down;
}
