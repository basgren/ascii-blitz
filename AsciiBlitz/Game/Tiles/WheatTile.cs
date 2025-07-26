using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class WheatTile : GroundTile {
  public override Sprite Sprite => SpriteRepo.Get<WheatTileSprite>();
}
