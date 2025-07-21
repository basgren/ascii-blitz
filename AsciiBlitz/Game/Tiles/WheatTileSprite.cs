using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Tiles;

public class WheatTileSprite() : Sprite(5, 3) {
  private const int damageThreshold = 1;
  
  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not WheatTile wheatTile) {
      return;
    }

    Vec2 fragPos = new(
      wheatTile.Pos.X * Width + context.CharPos.X,
      wheatTile.Pos.Y * Height + context.CharPos.Y
    );

    char? dmgChar = GroundTileHelper.GetDamageChar(context.CharPos.X, context.CharPos.Y, wheatTile, damageThreshold);

    if (dmgChar.HasValue) {
      cell.Char = dmgChar.Value;
      cell.Color = AnsiColor.Grayscale(Math.Clamp(wheatTile.DamageLevel, 1, 7));
    } else {
      if (wheatTile.DamageLevel >= damageThreshold) {
        cell.Char = '-';
        cell.Color = AnsiColor.Rgb(2, 2, 0);        
      } else {
        var intensity = GroundTileHelper.GetGrassColor((int)fragPos.X, (int)fragPos.Y, context.GameTime * 3, 0.5);
        
        cell.Char = '|';
        cell.Color = AnsiColor.Rgb(intensity + 3, intensity + 3, 0);        
      }
    }

    cell.BgColor = AnsiColor.Rgb(1, 1, 0);
  }
}
