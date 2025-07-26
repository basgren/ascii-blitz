namespace AsciiBlitz.Core.Types;

public readonly struct RectFloat {
  public readonly float X, Y, Width, Height;

  public RectFloat(float x, float y, float width, float height) {
    X = x;
    Y = y;
    Width = width;
    Height = height;
  }

  public RectFloat MoveTo(float x, float y) {
    return new RectFloat(x, y, Width, Height);
  }

  public bool Intersects(RectFloat other) {
    return !(X + Width <= other.X
             || other.X + other.Width <= X
             || Y + Height <= other.Y
             || other.Y + other.Height <= Y);
  }
}
