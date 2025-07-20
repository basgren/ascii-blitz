using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Game.Tiles;

public class RiverTile : TileObject {
  public override Sprite Sprite => SpriteRepo.Get<RiverTileSprite>(); 
  public int GrassDamageLevel = 0;

  public override void Visited() {
    GrassDamageLevel += 1;
  }
}
