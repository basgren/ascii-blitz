using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public class Projectile: MapUnitObject, ICollidable, IHasDamager {
  public override MapObjectType Type => MapObjectType.Projectile;
  public Vec2 Speed = Vec2.Zero;
  public float MaxTravelDistance = 5f;
  public IDamager Damager { get; } = new BaseDamager(1f);

  public RectFloat Bounds {
    get => new RectFloat(Pos.X, Pos.Y, 1f, 1f);
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

  private void MoveBy(Vec2 offset) {
    Pos += offset;
    _travelDistance += offset.Length;
  }
}
