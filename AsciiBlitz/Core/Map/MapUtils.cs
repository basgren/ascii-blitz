using AsciiBlitz.Core.Types;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Map;

public class MapUtils {
  public static Vec2Int PosToGrid(Vec2 pos) {
    return new Vec2Int((int)pos.X, (int)pos.Y);
  }
}
