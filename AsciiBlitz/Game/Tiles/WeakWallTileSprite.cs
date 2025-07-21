using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;

namespace AsciiBlitz.Game.Tiles;

public class WeakWallTileSprite() : Sprite(5, 3) {
  private static readonly string[] SpriteLines = [
    "ППППП",
    "ШШШШШ",
    "ППППП",
  ];

  private static readonly string DamageChars = "wvпш%";

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not WeakWallTile weakWallTile) {
      return;
    }
    
    float healthPercent = weakWallTile.Damageable.Health / weakWallTile.Damageable.MaxHealth;
    var pos = context.CharPos;
    
    // value is based on world position of wall tile to have different damage random for different tiles.
    int value = weakWallTile.Pos.X * 3 + weakWallTile.Pos.Y * 100 + pos.X + (pos.Y * Width);
    bool isPartDamaged = Rand(value) >= healthPercent;

    char c = isPartDamaged
      ? DamageChars[RandInt(value, DamageChars.Length)]
      : SpriteLines[pos.Y][pos.X];

    cell.Char = c;
  }
  
  private double Rand(int value) {
    double v = 22345f * (Math.Sin(value * 45678f) * 0.5f + 0.5f);
    
    return v - Math.Truncate(v);
  }
  
  private int RandInt(int value, int max) {
    return (int)Math.Floor(Rand(value) * max);
  }
}
