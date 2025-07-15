using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Game.Tiles;

public class EmptyTile : TileObject {
  public override Sprite2 Sprite => SpriteRepo.Get<EmptyTileSprite2>();
}
