using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Objects;

public class Tank : UnitObject, ICollidable, IDamageable {
  // public override MapObjectType Type => MapObjectType.Tank;

  public Vec2 GetShootPoint() {
    // TODO: actuially we should spawn bullet farther from tank barrel, but we should take into account that
    //   bullet is 1x1 char size in final view. but it means it has actual width = 1 / 3 and height = 1 / 3,
    //   as all objects are multiplied by 3 when displayed. In general we should take collision rect and use
    //   it here in calculations.
    int viewScale = 3; // temporary solition

    Vec2 offs = Dir switch {
      Direction.Up => new Vec2(0.5f, -1f / viewScale),
      Direction.Down => new Vec2(0.5f, 1),
      Direction.Left => new Vec2(-1f / viewScale, 0.5f),
      Direction.Right => new Vec2(1, 0.5f),
      _ => throw new ArgumentOutOfRangeException()
    };
    
    return Pos + offs;
  }

  public IDamageableComponent Damageable { get; } = new BaseDamageableComponent(3);
  public RectFloat Bounds => new RectFloat(Pos.X, Pos.Y, 1f, 1f);

  public bool IsActive { get; } = true;
  
  public void OnCollision(TileObject? tile) {
    // Do nothing.
  }

  public void OnCollision(ICollidable? tile) {
    // Do nothing
  }
}
