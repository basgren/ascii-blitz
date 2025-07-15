using AsciiBlitz.Core.Objects;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Tiles;

public class GrassTile(Vec2Int pos) : TileObject(pos) {
  public override MapObjectType Type => MapObjectType.Grass;
  public int GrassDamageLevel = 0;

  public override void Visited() {
    GrassDamageLevel += 1;
  }
}
