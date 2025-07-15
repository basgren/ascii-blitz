using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Game.Tiles;

public class GrassTile : TileObject {
  public override Sprite2 Sprite => SpriteRepo.Get<GrassTileSprite>(); 
  public int GrassDamageLevel = 0;

  public override void Visited() {
    GrassDamageLevel += 1;
  }
}
