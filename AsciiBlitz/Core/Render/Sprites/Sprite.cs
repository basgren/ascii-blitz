using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Render.Sprites;

public abstract class Sprite {
  public abstract char[,] GetChars(GameObject gameObject, double timeSeconds);
  public abstract char[,]? GetColors(GameObject gameObject, double timeSeconds);
}

public abstract class Sprite<T> : Sprite where T: GameObject {
  protected const int SpriteWidth = 3;
  protected const int SpriteHeight = 3;
  protected readonly char[,] Chars;
  protected readonly char[,] Colors;
    
  protected Sprite(string[] lines) {
    Chars = new char[3, 3];
    Colors = new char[3, 3];
    
    InitChars(lines);
  }

  protected void InitChars(string[] lines) {
    for (int y = 0; y < SpriteHeight; y++) {
      var line = lines[y].PadRight(3);
      
      for (int x = 0; x < SpriteWidth; x++) {
        Chars[x, y] = line[x];
      }
    }
  }
  
  protected void InitChars(Func<Vec2Int, char> charCallback) {
    for (int y = 0; y < SpriteHeight; y++) {
      for (int x = 0; x < SpriteWidth; x++) {
        Chars[x, y] = charCallback(new Vec2Int(x, y));
      }
    }
  }
  
  protected void InitColors(string[] lines) {
    for (int y = 0; y < 3; y++) {
      var line = lines[y].PadRight(3);
      
      for (int x = 0; x < 3; x++) {
        Colors[x, y] = line[x];
      }
    }
  }
  
  protected void InitColors(Func<Vec2Int, char> colorCallback) {
    for (int y = 0; y < SpriteHeight; y++) {
      for (int x = 0; x < SpriteWidth; x++) {
        Colors[x, y] = colorCallback(new Vec2Int(x, y));
      }
    }
  }

  public override char[,] GetChars(GameObject gameObject, double timeSeconds) {
    if (gameObject is T typedObject) {
      return GetChars(typedObject, timeSeconds);
    }

    return Chars;
  }

  public override char[,]? GetColors(GameObject gameObject, double timeSeconds) {
    if (gameObject is T typedObject) {
      return GetColors(typedObject, timeSeconds);
    }
    
    return null;
  }

  protected virtual char[,] GetChars(T mapObject, double timeSeconds) {
    return Chars;
  }

  protected virtual char[,]? GetColors(T mapObject, double timeSeconds) {
    return null;
  }
}