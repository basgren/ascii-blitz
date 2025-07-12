using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Objects;

public enum Direction {
  Up,
  Down,
  Left,
  Right,
}

public enum MapObjectType {
  Empty,
  Wall,
  Tank,
}

public abstract class MapObject {
  public abstract MapObjectType Type { get; }
}

public abstract class MapUnitObject() : MapObject() {
  public Vec2Int Pos;
  public Direction Dir = Direction.Down;
}

public class MapTank() : MapUnitObject() {
  public override MapObjectType Type => MapObjectType.Tank;
}

public class MapWall() : MapObject() {
  public override MapObjectType Type => MapObjectType.Wall;
}

public class MapEmpty() : MapObject() {
  public static readonly MapEmpty Instance = new();

  public override MapObjectType Type => MapObjectType.Empty;
}

public class MapObjectFactory {
  public MapObject Create(MapObjectType type) {
    switch (type) {
      case MapObjectType.Empty:
        return new MapEmpty();
      
      case MapObjectType.Wall:
        return new MapWall();
      
      case MapObjectType.Tank:
        return new MapTank();
      
      default:
        throw new ArgumentOutOfRangeException(nameof(type), type, null);
    }
    
  }
  
  public MapUnitObject CreateUnit(MapObjectType type) {
    switch (type) {
      case MapObjectType.Tank:
        return new MapTank();
      
      default:
        throw new ArgumentOutOfRangeException(nameof(type), type, null);
    }
    
  }
}
