using System.Text.RegularExpressions;

using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Render.Sprites;

public class WeakWallSprite : Sprite<WeakWallTile> {
  private static readonly string[] SpriteLines = [
    "ППП",
    "ШШШ",
    "ППП"
  ];

  private static readonly string DamageChars = "wvпш%";
    
  public WeakWallSprite() : base(SpriteLines) { }

  protected override char[,] GetChars(WeakWallTile mapObject, double timeSeconds) {
    float healthPercent = mapObject.Damageable.Health / mapObject.Damageable.MaxHealth;
    
    int width = Chars.GetLength(0);
    int height = Chars.GetLength(1);

    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        int value = mapObject.Pos.X * 3 + mapObject.Pos.Y * 100 + x + (y * width);
        bool isPartDamaged = Rand(value) >= healthPercent;

        char c = isPartDamaged
          ? DamageChars[RandInt(value, DamageChars.Length)]
          : SpriteLines[y][x];

        Chars[x, y] = c;
      }
    }
    
    return Chars;
  }
  
  private double Rand(int value) {
    double v = 22345f * (Math.Sin(value * 45678f) * 0.5f + 0.5f);
    
    return v - Math.Truncate(v);
  }
  
  private int RandInt(int value, int max) {
    return (int)Math.Floor(Rand(value) * max);
  }
}
