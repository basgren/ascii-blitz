using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class SoilTileSprite() : Sprite(5, 3) {
  private static readonly string Chars = ",.`";
  private const float SoilFillPercent = 0.3f;

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not SoilTile grassTile) {
      return;
    }
    
    char? dmgChar = GroundTileHelper.GetDamageChar(context.CharPos.X, context.CharPos.Y, grassTile, 1);

    if (!dmgChar.HasValue) {
      int fragY = grassTile.Pos.Y * Height + context.CharPos.Y;
      int fragX = grassTile.Pos.X * Width + context.CharPos.X;
      int id = fragY * 100 + fragX;

      cell.Char = ' ';
      double val = Rand(id);

      if (val <= SoilFillPercent) {
        int charId = (int)(val / SoilFillPercent * Chars.Length);
        cell.Char = Chars[charId]; 
      }

      cell.Color = AnsiColor.Grayscale(RandInt(id, 8) + 3);
    } else {
      cell.Char = dmgChar.Value;
      cell.Color = AnsiColor.Grayscale(Math.Clamp(grassTile.DamageLevel + 2, 2, 6));
    }
    
    cell.BgColor = AnsiColor.Grayscale(2);
  }

  private double Rand(int value) {
    double v = 12345.234f * (Math.Sin(value * 4567.89f) * 0.5f + 0.5f);

    return v - Math.Truncate(v);
  }

  private int RandInt(int value, int max) {
    return (int)Math.Floor(Rand(value) * max);
  }
}
