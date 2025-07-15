using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Objects;

public abstract class TileObject(Vec2Int pos) : GameObject {
  public readonly Vec2Int Pos = pos;
}
