using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class WallTile : TileObject {
  public override Sprite Sprite => SpriteRepo.Get<WallTileSprite>();
}
