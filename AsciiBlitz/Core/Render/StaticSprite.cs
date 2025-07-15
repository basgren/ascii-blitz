using AsciiBlitz.Core.Render.Buffer;

namespace AsciiBlitz.Core.Render;

public abstract class StaticSprite : Sprite2 {
  protected StaticSprite(string[] lines, int fgColor = 7, int bgColor = 0)
    : base(lines[0].Length, lines.Length) {
    for (int y = 0; y < Height; y++) {
      for (int x = 0; x < Width; x++) {
        char ch = lines[y][x];
        Cells[x, y] = new ScreenCell(ch, fgColor, bgColor);
      }
    }
  }

  public override void UpdateCell(in SpriteContext ctx, ref ScreenCell cell) {
    // Do nothing
  }
}
