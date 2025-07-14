using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public abstract class TileObject(Vec2Int pos) : GameObject {
  public readonly Vec2Int Pos = pos;
}
