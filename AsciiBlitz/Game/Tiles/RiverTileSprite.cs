using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Debug;

namespace AsciiBlitz.Game.Tiles;

public class RiverTileSprite() : Sprite(5, 3) {
  private static readonly string[] Chars = [
    "WVYwv ",
    "wwvv,.' ",
    "v.,.' ",
    ",.`   ",
    "#.`   ",
    "#.      ",
  ];

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not RiverTile tile) {
      return;
    }

    // cell.Char = '~';
    var samples = Chars[Math.Min(tile.GrassDamageLevel, Chars.Length - 1)];
    int id = (tile.Pos.Y * Height + context.CharPos.Y) * tile.Pos.X * Width * context.CharPos.X;
    int index = RandInt(id, samples.Length - 1);
    
    cell.Char = samples[index];
    
    
    if (tile.GrassDamageLevel >= 4) {
      cell.Color = AnsiColor.Grayscale(8);
    } else {
      var value = Math.Cos(context.GameTime / 3 + id) * 0.5 + 0.5;
    
      if (value < 0.3) {
        cell.Color = AnsiColor.Rgb(0, 0, 2);
      } else if (value < 0.6) {
        cell.Color = AnsiColor.Rgb(0, 0, 3);
      } else {
        cell.Color = AnsiColor.Rgb(0, 0, 4);
      }
    }
  }

  private double Rand(int value) {
    double v = 12345f * (Math.Sin(value * 45678f) * 0.5f + 0.5f);

    return v - Math.Truncate(v);
  }

  private int RandInt(int value, int max) {
    return (int)Math.Floor(Rand(value) * max);
  }
}
