namespace AsciiBlitz.Types;

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

  public static Vec2 Mix(Vec2 a, Vec2 b, float t) {
    return new Vec2(
      a.X + (b.X - a.X) * t,
      a.Y + (b.Y - a.Y) * t
    );
  }
}
