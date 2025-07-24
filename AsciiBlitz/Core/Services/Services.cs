namespace AsciiBlitz.Core.Services;

public static class Services {
  private static readonly Dictionary<Type, object> Instances = new();

  public static void Register<T>(T instance) where T : class {
    Instances[typeof(T)] = instance;
  }

  public static T Get<T>() where T : class {
    if (Instances.TryGetValue(typeof(T), out var instance)) {
      return (T)instance;
    }

    throw new InvalidOperationException($"Service {typeof(T)} not found");
  }
}
