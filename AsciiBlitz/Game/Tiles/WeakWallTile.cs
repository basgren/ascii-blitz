using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class WeakWallTile : TileObject, IDamageable {
  public IDamageableComponent Damageable { get; } = new BaseDamageableComponent(3);
  public override Sprite Sprite => SpriteRepo.Get<WeakWallTileSprite>();
}
