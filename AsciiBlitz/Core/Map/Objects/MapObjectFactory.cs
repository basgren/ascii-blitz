using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public class MapObjectFactory {
  public TileObject Tile(MapObjectType type, Vec2Int pos) {
    return type switch {
      MapObjectType.Empty => new EmptyTile(pos),
      MapObjectType.Wall => new WallTile(pos),
      MapObjectType.Grass => new GrassTile(pos),
      _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
  }
  
  public MapUnitObject Object(MapObjectType type) {
    return type switch {
      MapObjectType.Tank => new MapTank(),
      _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
  }
}
