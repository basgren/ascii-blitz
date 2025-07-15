namespace AsciiBlitz.Core.Objects;

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
  
  public event Action<GameObject>? OnDestroyed;
  public void Destroy() => OnDestroyed?.Invoke(this);

  public virtual void Visited() {
  }
}
