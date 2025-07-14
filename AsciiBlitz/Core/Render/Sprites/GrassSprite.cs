using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Render.Sprites;

public class GrassSprite : Sprite<GrassTile> {
  private static readonly string[] SpriteLines = [
    ",,,",
    ",,,",
    ",,,"
  ];
  
  private static readonly string[] SpriteColors = [
    "ggg",
    "ggg",
    "ggg"
  ];
  
  private static readonly string[] SpriteColors2 = [
    "xxx",
    "xxx",
    "xxx"
  ];

  public GrassSprite() : base(SpriteLines) {
    InitColors(SpriteColors);
  }

  private static readonly string[] _grassChars = [
    "YVWvwi   ",
    "ywv,.'  ",
    "vy.,.'  ",
    "v,.`    ",
    "#.`    ",
    "#.       ",
  ];

  protected override char[,] GetChars(GrassTile gameObject, double timeSeconds) {
    for (int y = 0; y < Height; y++) {
      for (int x = 0; x < Width; x++) {
        var samples = _grassChars[Math.Min(gameObject.GrassDamageLevel, _grassChars.Length - 1)];
        int index = RandInt((x + y * Height) * gameObject.Id, samples.Length - 1);
        
        Chars[x, y] = samples[index];
      }
    }
    
    return Chars;
  }

  protected override char[,] GetColors(GrassTile grass, double timeSeconds) {
    if (grass.GrassDamageLevel >= 4) {
      InitColors(SpriteColors2);
    } else {
      InitColors(delegate(Vec2Int pos) {
        // Just random periodic effect of wind on grass
        var value = Math.Cos(timeSeconds * (pos.X + pos.Y * Width) / 10 + grass.Id) * 0.5 + 0.5;
        
        return value < 0.5
          ? 'g'
          : 'G';
      });
    }
    
    return Colors;
  }

   
  private double Rand(int value) {
    double v = 12345f * (Math.Sin(value * 45678f) * 0.5f + 0.5f);
    
    return v - Math.Truncate(v);
  }
  
  private int RandInt(int value, int max) {
    return (int)Math.Floor(Rand(value) * max);
  }
}
