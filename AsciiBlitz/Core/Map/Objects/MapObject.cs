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

public abstract class MapTile(Vec2Int pos) : MapObject {
  public readonly Vec2Int Pos = pos;
}

public abstract class MapUnitObject() : MapObject() {
  public Vec2 Pos;
  public Direction Dir = Direction.Down;
}

public class MapTank() : MapUnitObject() {
  public override MapObjectType Type => MapObjectType.Tank;
}

public class MapWall(Vec2Int pos) : MapTile(pos) {
  public override MapObjectType Type => MapObjectType.Wall;
}

public class MapGrass(Vec2Int pos) : MapTile(pos) {
  public override MapObjectType Type => MapObjectType.Grass;
  public int GrassDamageLevel = 0;

  public override void Visited() {
    GrassDamageLevel += 1;
  }
}

public class MapEmpty(Vec2Int pos) : MapTile(pos) {
  public override MapObjectType Type => MapObjectType.Empty;
}

public class MapObjectFactory {
  public MapTile Tile(MapObjectType type, Vec2Int pos) {
    return type switch {
      MapObjectType.Empty => new MapEmpty(pos),
      MapObjectType.Wall => new MapWall(pos),
      MapObjectType.Grass => new MapGrass(pos),
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
