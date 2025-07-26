namespace AsciiBlitz.Core.Types;

public enum Direction {
  Up,
  Down,
  Left,
  Right,
}

public static class DirectionExtensions {
  public static Vec2Int ToVec2Int(this Direction dir) {
    return dir switch {
      Direction.Up => Vec2Int.Up,
      Direction.Down => Vec2Int.Down,
      Direction.Left => Vec2Int.Left,
      Direction.Right => Vec2Int.Right,
      _ => Vec2Int.Zero,
    };
  }

  public static Vec2 ToVec2(this Direction dir) {
    return dir switch {
      Direction.Up => Vec2.Up,
      Direction.Down => Vec2.Down,
      Direction.Left => Vec2.Left,
      Direction.Right => Vec2.Right,
      _ => Vec2.Zero,
    };
  }

  public static Direction Opposite(this Direction dir) {
    return dir switch {
      Direction.Up => Direction.Down,
      Direction.Down => Direction.Up,
      Direction.Left => Direction.Right,
      Direction.Right => Direction.Left,
      _ => dir,
    };
  }

  public static Direction TurnCw(this Direction dir) {
    return dir switch {
      Direction.Up => Direction.Right,
      Direction.Right => Direction.Down,
      Direction.Down => Direction.Left,
      Direction.Left => Direction.Up,
      _ => dir,
    };
  }

  public static Direction TurnCcw(this Direction dir) {
    return dir switch {
      Direction.Up => Direction.Left,
      Direction.Right => Direction.Up,
      Direction.Down => Direction.Right,
      Direction.Left => Direction.Down,
      _ => dir,
    };
  }
}
