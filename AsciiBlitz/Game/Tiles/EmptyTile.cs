using AsciiBlitz.Core.Objects;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Tiles;

public class EmptyTile(Vec2Int pos) : TileObject(pos) {
  public override MapObjectType Type => MapObjectType.Empty;
}
