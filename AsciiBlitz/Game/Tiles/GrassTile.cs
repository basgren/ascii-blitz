using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Game.Tiles;

public class GrassTile : GroundTile {
  public override Sprite Sprite => SpriteRepo.Get<GrassTileSprite>();
}
