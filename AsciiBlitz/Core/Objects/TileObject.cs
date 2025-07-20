using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Objects;

public abstract class TileObject : GameObject {
  public Vec2Int Pos { get; private set; } = Vec2Int.Zero;

  public void SetPos(Vec2Int pos) {
    Pos = pos;
  }

  public virtual bool CollidesWith(GameObject obj) {
    return true;
  }
}
