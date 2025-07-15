using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Types;
using AsciiBlitz.Game.Objects;
using AsciiBlitz.Game.Tiles;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Map.Objects;

public class MapObjectFactory {
  public TileObject Tile(MapObjectType type, Vec2Int pos) {
    return type switch {
      MapObjectType.Empty => new EmptyTile(pos),
      MapObjectType.Wall => new WallTile(pos),
      MapObjectType.WeakWall => new WeakWallTile(pos),
      MapObjectType.Grass => new GrassTile(pos),
      _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
  }
  
  public UnitObject Object(MapObjectType type) {
    return type switch {
      MapObjectType.Tank => new Tank(),
      _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
  }
}
