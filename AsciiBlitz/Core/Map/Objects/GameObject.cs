namespace AsciiBlitz.Core.Map.Objects;

public enum MapObjectType {
  Empty,
  Wall,
  WeakWall,
  Tank,
  Grass,
  Projectile,
}

public abstract class GameObject {
  private static int _nextId = 1;
  public readonly int Id = _nextId++;
  
  public abstract MapObjectType Type { get; }

  public virtual void Visited() {
  }
}
