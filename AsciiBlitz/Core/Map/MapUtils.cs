using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Map;

public static class MapUtils {
  /// <summary>
  /// Convert floating point coordinates to grid coordinates, which then can be used to get
  /// info about tiles and occupied cells.
  /// As our current model is very simple, the whole conversion is just truncating decimals. 
  /// </summary>
  /// <param name="pos"></param>
  /// <returns></returns>
  public static Vec2Int PosToGrid(Vec2 pos) {
    return new Vec2Int((int)pos.X, (int)pos.Y);
  }
}
