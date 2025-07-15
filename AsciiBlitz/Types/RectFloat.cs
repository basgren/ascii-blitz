namespace AsciiBlitz.Types;

public struct RectFloat
{
  public float X, Y, Width, Height;

  public RectFloat(float x, float y, float width, float height)
  {
    X = x;
    Y = y;
    Width = width;
    Height = height;
  }

  public void MoveTo(float x, float y)
  {
    X = x;
    Y = y;
  }

  public void Offset(float dx, float dy)
  {
    X += dx;
    Y += dy;
  }

  public bool Intersects(RectFloat other)
  {
    return !(X + Width <= other.X
             || other.X + other.Width <= X
             || Y + Height <= other.Y
             || other.Y + other.Height <= Y);
  }
}
