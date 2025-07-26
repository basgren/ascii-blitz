namespace AsciiBlitz.Core.Types;

public record struct Vec2Int(int X, int Y) {
  public static Vec2Int Zero => new(0, 0);
  public static Vec2Int Up => new(0, -1);
  public static Vec2Int Down => new(0, 1);
  public static Vec2Int Left => new(-1, 0);
  public static Vec2Int Right => new(1, 0);
  
  public static Vec2Int operator +(Vec2Int a, Vec2Int b) {
    return new Vec2Int(a.X + b.X, a.Y + b.Y);
  }
    
  public static Vec2Int operator -(Vec2Int a, Vec2Int b) {
    return new Vec2Int(a.X - b.X, a.Y - b.Y);
  }
  
  public static Vec2Int operator *(Vec2Int v, int scalar) => new(v.X * scalar, v.Y * scalar);
  public static Vec2Int operator *(int scalar, Vec2Int v) => new(v.X * scalar, v.Y * scalar);

  public override string ToString() {
    return $"[{X}, {Y}]";
  }
}
