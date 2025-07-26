using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class GrassTile : GroundTile {
  public override Sprite Sprite => SpriteRepo.Get<GrassTileSprite>();
}
