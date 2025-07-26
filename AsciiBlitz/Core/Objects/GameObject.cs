using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Sprites;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Objects;

public abstract class GameObject : IUpdatable {
  private static int _nextId = 1;
  public readonly int Id = _nextId++;
  
  // TODO probably should be moved to subclass, as particle system doesn't need sprite.
  public virtual Sprite? Sprite => null;
  
  public event Action<GameObject>? OnDestroyed;

  public virtual void OnBeforeDestroy() {
  }

  public void Destroy() => OnDestroyed?.Invoke(this);

  public virtual void Update(float deltaTime) {
  }
}
