namespace AsciiBlitz.Core.Types;

public interface IUpdatable {
  /// <summary>
  /// All objects that should be updated every frame, should implement this interface.
  /// </summary>
  /// <param name="deltaTime">Time in seconds (game time) that have passed since the previous frame.</param>
  void Update(float deltaTime);
}
