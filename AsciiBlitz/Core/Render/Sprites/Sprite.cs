namespace AsciiBlitz.Core.Render.Sprites;

public abstract class Sprite {
  private readonly char[,] _spriteData;
    
  protected Sprite(string[] lines) {
    if (lines.Length != 3) {
      throw new ArgumentException("Sprite must have exactly 3 lines");
    }
        
    _spriteData = new char[3, 3];
    PopulateSpriteData(lines);
  }
    
  private void PopulateSpriteData(string[] lines) {
    for (int y = 0; y < 3; y++) {
      var line = lines[y].PadRight(3);
      for (int x = 0; x < 3; x++) {
        _spriteData[x, y] = line[x];
      }
    }
  }
    
  public char[,] GetSprite() => _spriteData;
}

public class EmptySprite : Sprite {
  private static readonly string[] SpriteLines = {
    "   ",
    "   ",
    "   "
  };
    
  public EmptySprite() : base(SpriteLines) { }
}

public class WallSprite : Sprite {
  private static readonly string[] SpriteLines = {
    "███",
    "███",
    "███"
  };
    
  public WallSprite() : base(SpriteLines) { }
}

public class UnknownSprite : Sprite {
  private static readonly string[] SpriteLines = {
    "???",
    "???",
    "???"
  };
    
  public UnknownSprite() : base(SpriteLines) { }
}
