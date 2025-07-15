namespace AsciiBlitz.Types;

public record struct Vec2(float X, float Y) {
  public static Vec2 Zero => new(0, 0);
  public static Vec2 Up => new(0, -1);
  public static Vec2 Down => new(0, 1);
  public static Vec2 Left => new(-1, 0);
  public static Vec2 Right => new(1, 0);
  
  public static Vec2 operator +(Vec2 a, Vec2 b) {
    return new Vec2(a.X + b.X, a.Y + b.Y);
  }
    
  public static Vec2 operator -(Vec2 a, Vec2 b) {
    return new Vec2(a.X - b.X, a.Y - b.Y);
  }
  
  public static Vec2 operator *(Vec2 v, float scalar) => new(v.X * scalar, v.Y * scalar);
  public static Vec2 operator *(float scalar, Vec2 v) => new(v.X * scalar, v.Y * scalar);
  
  public static Vec2 operator /(Vec2 v, float scalar) => new(v.X / scalar, v.Y / scalar);
  
  public float Length => MathF.Sqrt(X * X + Y * Y);
  public bool IsZero => X == 0 && Y == 0;
}
