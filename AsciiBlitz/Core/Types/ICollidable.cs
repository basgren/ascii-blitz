using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Types;

public interface ICollidable {
  RectFloat Bounds { get; }
  bool IsActive { get; }
  void OnCollision(TileObject? tile);
  void OnCollision(ICollidable? tile);
}
