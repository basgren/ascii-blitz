using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Objects;

public static class TileFactory {
  public static T Create<T>(Vec2Int pos) where T : TileObject, new() {
    T tile = new();
    tile.SetPos(pos);
    return tile;
  }
}
