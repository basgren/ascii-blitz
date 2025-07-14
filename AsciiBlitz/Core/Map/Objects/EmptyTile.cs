using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public class EmptyTile(Vec2Int pos) : TileObject(pos) {
  public override MapObjectType Type => MapObjectType.Empty;
}
