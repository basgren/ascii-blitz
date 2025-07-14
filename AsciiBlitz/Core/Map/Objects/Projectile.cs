using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public class Projectile: MapUnitObject {
  public override MapObjectType Type => MapObjectType.Projectile;
  public Vec2 Speed = Vec2.Zero;
  public float MaxTravelDistance = 5f;

  private float _travelDistance = 0;

  public override void Update(float deltaTime) {
    if (!Speed.IsZero) {
      MoveBy(Speed * deltaTime);
      
      if (_travelDistance >= MaxTravelDistance) {
        Destroy();
      }
    }
  }

  public void MoveBy(Vec2 offset) {
    Pos += offset;
    _travelDistance += offset.Length;
  }
}
