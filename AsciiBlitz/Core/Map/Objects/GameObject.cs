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

public abstract class GameObject {
  private static int _nextId = 1;
  public readonly int Id = _nextId++;
  
  public abstract MapObjectType Type { get; }

  public virtual void Visited() {
  }
}
