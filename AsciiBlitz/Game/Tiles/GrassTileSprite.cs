using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Debug;

namespace AsciiBlitz.Game.Tiles;

public class GrassTileSprite : Sprite {
  public GrassTileSprite() : base(5, 3) {
  }
  
  private const int damageThreshold = 4;

  private static readonly string[] _grassChars = [
    "WVYwv ",
    "wwvv,.' ",
    "v.,.' ",
    ",.`  ",
  ];

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not GrassTile grassTile) {
      return;
    }

    int fragY = grassTile.Pos.Y * Height + context.CharPos.Y;
    int fragX = grassTile.Pos.X * Width + context.CharPos.X;
    
    int id = (fragY + fragX) * 100 * fragX;

    char? dmgChar = GroundTileHelper.GetDamageChar(context.CharPos.X, context.CharPos.Y, grassTile, damageThreshold);

    if (dmgChar.HasValue) {
      cell.Char = dmgChar.Value;
      cell.Color = AnsiColor.Grayscale(Math.Clamp(grassTile.DamageLevel, 1, 7));
    } else {
      var samples = _grassChars[Math.Min(grassTile.DamageLevel, _grassChars.Length - 1)];
      int index = RandInt(id, samples.Length);
      
      cell.Char = samples[index];

      if (grassTile.DamageLevel < damageThreshold) {
        var i = GroundTileHelper.GetGrassColor(fragX, fragY, context.GameTime * 3, 1);
        cell.Color = AnsiColor.Rgb(0, i + 2, 0);        
      } else {
        cell.Color = AnsiColor.Rgb(0, 2, 0);
      }
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
