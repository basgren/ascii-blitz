using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;

namespace AsciiBlitz.Game.Tiles;

public class GrassTileSprite : Sprite {
  public GrassTileSprite() : base(3, 3) {
  }

  private static readonly string[] _grassChars = [
    "WVYwv ",
    "wwvv,.' ",
    "v.,.' ",
    ",.`   ",
    "#.`   ",
    "#.      ",
  ];

  public override void UpdateCell(in SpriteContext context, ref ScreenCell cell) {
    if (context.GameObject is not GrassTile grassTile) {
      return;
    }

    var samples = _grassChars[Math.Min(grassTile.GrassDamageLevel, _grassChars.Length - 1)];
    int index = RandInt((context.CharPos.X + context.CharPos.Y * Height) * grassTile.Id, samples.Length - 1);

    cell.Char = samples[index];


    if (grassTile.GrassDamageLevel >= 4) {
      cell.Color = AnsiColor.Grayscale(8);
    } else {
      var value = Math.Cos(context.GameTime * (context.CharPos.X + context.CharPos.Y * Width) / 10
                           + grassTile.Id) * 0.5 + 0.5;

      cell.Color = value < 0.5 ? AnsiColor.Green : AnsiColor.BrightGreen;
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
