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
  Grass,
}

public abstract class MapObject {
  private static int _nextId = 1;
  public readonly int Id = _nextId++;
  
  public abstract MapObjectType Type { get; }

  public virtual void Visited() {
  }
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

public class MapGrass() : MapObject() {
  public override MapObjectType Type => MapObjectType.Grass;
  public int GrassDamageLevel = 0;

  public override void Visited() {
    GrassDamageLevel += 1;
  }
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
      
      case MapObjectType.Grass:
        return new MapGrass();
      
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
