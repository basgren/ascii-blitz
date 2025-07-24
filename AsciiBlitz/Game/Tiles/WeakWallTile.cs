using AsciiBlitz.Core.Audio;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class WeakWallTile : TileObject, IDamageable {
  public IDamageableComponent Damageable { get; } = new BaseDamageableComponent(3);
  public override Sprite Sprite => SpriteRepo.Get<WeakWallTileSprite>();

  public override void Destroy() {
    base.Destroy();
    GameSoundService.Instance.PlaySound("rocks-falling.wav");
  }
}
