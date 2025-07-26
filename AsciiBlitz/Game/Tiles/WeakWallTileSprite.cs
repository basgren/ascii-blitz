using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class WeakWallTileSprite() : Sprite(5, 3) {
  private static readonly string CrackChars = ",'.-";
  private static readonly string DamageChars = ";:\"-!/\\";

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not WeakWallTile tile) {
      return;
    }
    
    float healthPercent = tile.Damageable.Health / tile.Damageable.MaxHealth;
    var pos = context.CharPos;
    
    // value is based on world position of wall tile to have different damage random for different tiles.
    int fragPosX = tile.Pos.X * Width + context.CharPos.X; 
    int fragPosY = tile.Pos.Y * Height + context.CharPos.Y;
    int fragId = fragPosY * 100 + fragPosX;
    
    
    bool isPartDamaged = Rand(fragId * 2) >= healthPercent;

    char c = ' ';

    if (isPartDamaged) {
      c = DamageChars[RandInt(fragId, DamageChars.Length)];
      cell.Color = AnsiColor.Grayscale(8 - (int)((1 - healthPercent) * 6));
    } else {
      if (Rand(fragId * 15) < 0.3) {
        int index = RandInt(fragId, CrackChars.Length);
        c = CrackChars[index];
        cell.Color = AnsiColor.Grayscale(8);
      }
    }

    cell.Char = c;
    cell.BgColor = WallTileSprite.BaseColor;
  }
  
  private double Rand(int value) {
    double v = 22345f * (Math.Sin(value * 45678f) * 0.5f + 0.5f);
    
    return v - Math.Truncate(v);
  }
  
  private int RandInt(int value, int max) {
    return (int)Math.Floor(Rand(value) * max);
  }
}
