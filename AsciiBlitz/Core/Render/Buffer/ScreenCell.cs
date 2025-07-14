namespace AsciiBlitz.Core.Render.Buffer;

public struct ScreenCell {
  public char Char;
  public int Color; // 0–255 ANSI
  public int BgColor;

  public ScreenCell(char @char, int fg, int bg) {
    Char = @char;
    Color = fg;
    BgColor = bg;
  }

  public bool Equals(ScreenCell other) {
    return Char == other.Char &&
           Color == other.Color &&
           BgColor == other.BgColor;
  }

  public override bool Equals(object? obj) => obj is ScreenCell other && Equals(other);

  public override int GetHashCode() =>
    HashCode.Combine(Char, Color, BgColor);

  public static bool operator ==(ScreenCell a, ScreenCell b) => a.Equals(b);
  public static bool operator !=(ScreenCell a, ScreenCell b) => !a.Equals(b);
}
