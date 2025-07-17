using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Types;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Objects;

public class Projectile : UnitObject, ICollidable, IDamager {
  // public override MapObjectType Type => MapObjectType.Projectile;
  public Vec2 Speed = Vec2.Zero;
  public float MaxTravelDistance = 5f;
  public IDamagerComponent Damager { get; } = new BaseDamagerComponent(1f);
  public override Sprite Sprite => SpriteRepo.Get<ProjectileSprite>();
  public RectFloat Bounds => new(Pos.X, Pos.Y, Size.X, Size.Y);

  public Projectile() {
    Size = new Vec2(1f / 3, 1f / 3);
  }

  public bool IsActive { get; private set; } = true;

  private float _travelDistance = 0;


  public override void Update(float deltaTime) {
    if (!Speed.IsZero) {
      MoveBy(Speed * deltaTime);

      if (_travelDistance >= MaxTravelDistance) {
        Destroy();
      }
    }
  }

  public void OnCollision(TileObject? tile) {
    Destroy();
  }

  public void OnCollision(ICollidable? tile) {
    // TODO: do we need 2 OnCollision handlers? Think later - maybe unite handling of Tiles and Objects
    Destroy();
  }

  private void MoveBy(Vec2 offset) {
    Pos += offset;
    _travelDistance += offset.Length;
  }
}
