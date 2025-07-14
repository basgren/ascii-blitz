using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public class MapTank() : MapUnitObject() {
  public override MapObjectType Type => MapObjectType.Tank;

  public Vec2 GetShootPoint() {
    Vec2 offs = Dir switch {
      Direction.Up => new Vec2(0.5f, -0.01f),
      Direction.Down => new Vec2(0.5f, 1),
      Direction.Left => new Vec2(-0.01f, 0.5f),
      Direction.Right => new Vec2(1, 0.5f),
      _ => throw new ArgumentOutOfRangeException()
    };
    
    return Pos + offs;
  }
}
