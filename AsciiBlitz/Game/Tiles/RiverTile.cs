using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Game.Objects;

namespace AsciiBlitz.Game.Tiles;

public class RiverTile : TileObject {
  public override Sprite Sprite => SpriteRepo.Get<RiverTileSprite>();
  public override bool CollidesWith(GameObject obj) {
    if (obj is Projectile) {
      return false;
    }

    return base.CollidesWith(obj);
  }
}
