using AsciiBlitz.Core.Objects;

namespace AsciiBlitz.Core.Map.Layers;

// Non-generic base for polymorphic collections
public abstract class AbstractMapLayer(int id, int order) {
  public int Id { get; } = id;
  public int Order { get; } = order;

  public abstract void Resize(int width, int height);
  public abstract IReadOnlyList<GameObject> GetAllObjects();
  public abstract void Add(GameObject obj);
  public abstract void Remove(GameObject obj);
}

public abstract class AbstractMapLayer<T>(int id, int order) : AbstractMapLayer(id, order)
  where T : GameObject {
  public int Id { get; } = id;
  public int Order { get; } = order;

  /// <summary>
  /// Returns all objects on the layer that should be rendered.
  /// </summary>
  /// <returns></returns>
  public abstract IReadOnlyList<T> GetObjects();

  public abstract void Add(T obj);

  public abstract void Remove(T obj);

  // Implement base class methods by delegating to type-safe versions
  public sealed override IReadOnlyList<GameObject> GetAllObjects() {
    // Seems there are some issues with polymorphic methods - we can't declare both
    // `IReadOnlyList<T> GetAllObjects()` and `IReadOnlyList<MapObject> GetObjects()` as they don't have
    // different arguments. That's why we have to choose different name for generalized method - `GetObjects()`
    // and derived classes should override it. This method we'll mark as sealed to avoid further overriding.
    // And in general in consumers it's better to use `GetObjects()` method on a specific layer type.
    return GetObjects().Cast<GameObject>().ToList();
  }

  public override void Add(GameObject obj) {
    if (obj is T typedObj) {
      Add(typedObj);
    }
    else {
      throw new ArgumentException($"Cannot add {obj.GetType().Name} to {typeof(T).Name} layer");
    }
  }

  public override void Remove(GameObject obj) {
    if (obj is T typedObj) {
      Remove(typedObj);
    }
  }
}
