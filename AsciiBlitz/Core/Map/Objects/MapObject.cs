namespace AsciiBlitz.Core.Map.Objects;

public enum MapObjectType {
  Empty,
  Wall,
}

public abstract class MapObject {
  public abstract MapObjectType Type { get; }
}

public class MapWall() : MapObject() {
  public override MapObjectType Type => MapObjectType.Wall;
}

public class MapEmpty() : MapObject() {
  public override MapObjectType Type => MapObjectType.Empty;
}

public class MapObjectFactory {
  public MapObject Create(MapObjectType type) {
    switch (type) {
      case MapObjectType.Empty:
        return new MapEmpty();
      
      case MapObjectType.Wall:
        return new MapWall();
      
      default:
        throw new ArgumentOutOfRangeException(nameof(type), type, null);
    }
    
  }
}
