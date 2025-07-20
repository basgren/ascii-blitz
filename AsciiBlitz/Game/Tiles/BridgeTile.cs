using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Game.Tiles;

public class BridgeTile : TileObject {
  public override Sprite Sprite => SpriteRepo.Get<BridgeTileSprite>(); 
  public int GrassDamageLevel = 0;

  public override void Visited() {
    GrassDamageLevel += 1;
  }
}
