using AsciiBlitz.Core.Objects;

namespace AsciiBlitz.Game.Tiles;

public class GrassTile : TileObject {
  public int GrassDamageLevel = 0;

  public override void Visited() {
    GrassDamageLevel += 1;
  }
}
