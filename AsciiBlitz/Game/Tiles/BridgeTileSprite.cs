using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Debug;

namespace AsciiBlitz.Game.Tiles;

public class BridgeTileSprite() : Sprite(5, 3) {
  private static readonly string[] Chars = [
    "WVYwv ",
    "wwvv,.' ",
    "v.,.' ",
    ",.`   ",
    "#.`   ",
    "#.      ",
  ];

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {

    cell.Char = '|';
    // cell.BgColor = AnsiColor.Rgb(1, 1, 1);
    // 94, 130, 136
    cell.BgColor = 94;
    cell.Color = 136;
  }
}
