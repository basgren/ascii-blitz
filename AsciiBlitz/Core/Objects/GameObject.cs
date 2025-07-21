using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Core.Objects;

// public enum MapObjectType {
//   Empty,
//   Wall,
//   WeakWall,
//   Tank,
//   Grass,
//   Projectile,
// }

public abstract class GameObject {
  private static int _nextId = 1;
  public readonly int Id = _nextId++;
  
  public virtual Sprite? Sprite => null;
  
  // public abstract MapObjectType Type { get; }
  
  public event Action<GameObject>? OnDestroyed;
  public void Destroy() => OnDestroyed?.Invoke(this);
}
