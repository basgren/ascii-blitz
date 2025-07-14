using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public class WeakWallTile(Vec2Int pos) : TileObject(pos), IHasDamageable {
  public override MapObjectType Type => MapObjectType.WeakWall;
  public IDamageable Damageable { get; } = new BaseDamageable(5);
}
