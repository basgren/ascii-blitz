using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Tiles;

public class WheatTileSprite : Sprite {
  public WheatTileSprite() : base(5, 3) {
  }

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not WheatTile wheatTile) {
      return;
    }

    Vec2 fragPos = new(
      wheatTile.Pos.X * Width + context.CharPos.X,
      wheatTile.Pos.Y * Height + context.CharPos.Y
    );

    var intensity = GrassTileHelper.GetGrassColor((int)fragPos.X, (int)fragPos.Y, context.GameTime * 3, 0.5);

    cell.Char = '|';
    cell.Color = AnsiColor.Rgb(intensity + 3, intensity + 3, 0);
    cell.BgColor = AnsiColor.Rgb(1, 1, 0);
  }
}
