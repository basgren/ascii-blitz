using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;

namespace AsciiBlitz.Game.Tiles;

public class SoilTileSprite : Sprite {
  public SoilTileSprite() : base(5, 3) {
  }

  private static readonly string _chars = ",.'`-";
  private const float SoilFillPercent = 0.1f;

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not SoilTile grassTile) {
      return;
    }
    
    int fragY = grassTile.Pos.Y * Height + context.CharPos.Y;
    int fragX = grassTile.Pos.X * Width + context.CharPos.X;
    int id = fragY * 100 + fragX;

    cell.Char = ' ';
    double val = Rand(id);

    if (val <= SoilFillPercent) {
      int charId = (int)(val / SoilFillPercent * _chars.Length);
      cell.Char = _chars[charId]; 
    }
    
    cell.BgColor = AnsiColor.Grayscale(1);
    cell.Color = AnsiColor.Grayscale(RandInt(id, 8) + 3);
  }

  private double Rand(int value) {
    double v = 12345.234f * (Math.Sin(value * 4567.89f) * 0.5f + 0.5f);

    return v - Math.Truncate(v);
  }

  private int RandInt(int value, int max) {
    return (int)Math.Floor(Rand(value) * max);
  }
}
