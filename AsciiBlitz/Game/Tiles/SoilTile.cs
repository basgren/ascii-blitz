using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Game.Tiles;

public class SoilTile : TileObject {
  public override Sprite Sprite => SpriteRepo.Get<SoilTileSprite>(); 
  public int GrassDamageLevel = 0;

  public override void Visited() {
    GrassDamageLevel += 1;
  }
}
