using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;

using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Tiles;

public class RiverTileSprite() : Sprite(5, 3) {
  private static Vec2 _pivotPoint = new(-300, 0);

  private static readonly int[] Colors = [
    AnsiColor.Rgb(0, 0, 2),
    AnsiColor.Rgb(0, 0, 3),
    AnsiColor.Rgb(0, 0, 4),
  ];

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not RiverTile tile) {
      return;
    }

    Vec2 fragPos = new(
      tile.Pos.X * Width + context.CharPos.X,
      tile.Pos.Y * Height + context.CharPos.Y
    );

    float dist = _pivotPoint.Distance(fragPos);
    float a = (float)Math.Sin(fragPos.Y + fragPos.X) * 0.5f + 0.5f;
    int phase = (int)MathF.Round(MathF.Sin((float)(context.GameTime * 2 + dist)) + a * 5) % Colors.Length;

    if (phase < 0) {
      phase = 0;
    }
    
    cell.Char = '~';
    cell.Color = Colors[phase];
    cell.BgColor = AnsiColor.Rgb(0, 0, 2);
  }
}
