using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public class Projectile: MapUnitObject, ICollidable, IHasDamager {
  public override MapObjectType Type => MapObjectType.Projectile;
  public Vec2 Speed = Vec2.Zero;
  public float MaxTravelDistance = 5f;
  public IDamager Damager { get; } = new BaseDamager(1f);

  public RectFloat Bounds {
    // TODO: take size from associated sprite
    get => new RectFloat(Pos.X, Pos.Y, 1f / 3, 1f / 3);
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
