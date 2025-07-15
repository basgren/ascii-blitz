namespace AsciiBlitz.Core.Render;

public static class SpriteRepo {
  private static readonly Dictionary<Type, Sprite> Cache = new();

  public static T Get<T>() where T : Sprite, new() {
    var type = typeof(T);

    if (Cache.TryGetValue(type, out var sprite)) {
      return (T)sprite;
    }

    var instance = new T();
    Cache[type] = instance;

    return instance;
  }
}
