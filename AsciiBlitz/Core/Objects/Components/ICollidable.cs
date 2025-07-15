using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Objects.Components;

// TODO: think about making it also a component
public interface ICollidable {
  RectFloat Bounds { get; }
  bool IsActive { get; }
  void OnCollision(TileObject? tile);
  void OnCollision(ICollidable? tile);
}
