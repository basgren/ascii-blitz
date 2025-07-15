using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Game.Tiles;

public class WeakWallTile : TileObject, IDamageable {
  public IDamageableComponent Damageable { get; } = new BaseDamageableComponent(5);
  public override Sprite2 Sprite => SpriteRepo.Get<WeakWallTileSprite2>();
}
