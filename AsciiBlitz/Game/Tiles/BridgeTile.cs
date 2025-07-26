using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class BridgeTile : TileObject {
  public override Sprite Sprite => SpriteRepo.Get<BridgeTileSprite>(); 
}
