using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;

using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Tiles;

public class RiverTileSprite() : Sprite(5, 3) {
  private static Vec2 pivotPoint = new(-300, 0);

  private static int[] _colors = [
    AnsiColor.Rgb(0, 0, 2),
    AnsiColor.Rgb(0, 0, 3),
    AnsiColor.Rgb(0, 0, 4),
    // AnsiColor.Rgb(3, 3, 0),
  ];

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not RiverTile tile) {
      return;
    }

    Vec2 fragPos = new(
      tile.Pos.X * Width + context.CharPos.X,
      tile.Pos.Y * Height + context.CharPos.Y
    );

    float fx = 0.12f;
    float fy = 0.14f;
    float dist = pivotPoint.Distance(fragPos);
    float a = (float)Math.Sin(fragPos.Y + fragPos.X) * 0.5f + 0.5f;
    int sway = (int)MathF.Round(MathF.Sin((float)(context.GameTime * 2 + dist)) + a * 5) % _colors.Length;

    if (sway < 0) {
      sway = 0;
    }
    
    cell.Char = '~';
    cell.Color = _colors[sway];
    cell.BgColor = AnsiColor.Rgb(0, 0, 2);
  }
}
