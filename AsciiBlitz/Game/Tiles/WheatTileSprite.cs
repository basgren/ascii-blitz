using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Tiles;

public class WheatTileSprite : Sprite {
  private static Vec2 pivotPoint = new(34, 50);

  public WheatTileSprite() : base(5, 3) {
  }

  // private static readonly string Chars = "\\|/";
  private static readonly string Chars = "\\!!";

  private static int[] _colors = [
    AnsiColor.Rgb(4, 4, 0),
    AnsiColor.Rgb(5, 5, 0),
    AnsiColor.Rgb(5, 5, 0),
    // AnsiColor.Rgb(3, 3, 0),
  ];

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not WheatTile wheatTile) {
      return;
    }

    Vec2 fragPos = new(
      wheatTile.Pos.X * Width + context.CharPos.X,
      wheatTile.Pos.Y * Height + context.CharPos.Y
    );

    float fx = 0.12f;
    float fy = 0.14f;
    int sway = (int)MathF.Round(-MathF.Sin((float)context.GameTime / 3 + fragPos.X * fx + fragPos.Y * fy)) + 1;

    cell.Char = '!'; 

    if (wheatTile.GrassDamageLevel >= 4) {
      cell.Color = AnsiColor.Grayscale(8);
    } else {
      cell.Color = _colors[sway];
      cell.BgColor = AnsiColor.Rgb(1, 1, 0);
    }
  }
}
