using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public class WallTile(Vec2Int pos) : TileObject(pos) {
  public override MapObjectType Type => MapObjectType.Wall;
}
