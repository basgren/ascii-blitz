using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Types;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Tiles;

public class WeakWallTile(Vec2Int pos) : TileObject(pos), IDamageable {
  public override MapObjectType Type => MapObjectType.WeakWall;
  public IDamageableComponent Damageable { get; } = new BaseDamageableComponent(5);
}
