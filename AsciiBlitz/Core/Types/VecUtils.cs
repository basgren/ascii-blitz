namespace AsciiBlitz.Core.Types;

public static class VecUtils {
  public static Vec2 DirToVec2(Direction dir) {
    return dir switch {
      Direction.Up => Vec2.Up,
      Direction.Down => Vec2.Down,
      Direction.Left => Vec2.Left,
      Direction.Right => Vec2.Right,
      _ => Vec2.Zero
    };
  }
}
