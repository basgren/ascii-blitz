using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Render.Sprites;

public abstract class Sprite {
  public abstract char[,] GetSprite(MapObject mapObject);
}

public abstract class Sprite<T> : Sprite where T: MapObject {
  protected readonly char[,] Chars;
    
  protected Sprite(string[] lines) {
    Chars = new char[3, 3];
    InitChars(lines);
  }

  protected void InitChars(string[] lines) {
    for (int y = 0; y < 3; y++) {
      var line = lines[y].PadRight(3);
      
      for (int x = 0; x < 3; x++) {
        Chars[x, y] = line[x];
      }
    }
  }

  public override char[,] GetSprite(MapObject mapObject) {
    if (mapObject is T typedObject) {
      return GetSprite(typedObject);
    }
    
    // Return default sprite for wrong type
    return Chars;
  }

  public virtual char[,] GetSprite(T mapObject) {
    return Chars;
  }
}

public class EmptySprite : Sprite<MapEmpty> {
  private static readonly string[] SpriteLines = {
    "   ",
    "   ",
    "   "
  };
    
  public EmptySprite() : base(SpriteLines) { }
}

public class WallSprite : Sprite<MapWall> {
  private static readonly string[] SpriteLines = {
    "███",
    "███",
    "███"
  };
    
  public WallSprite() : base(SpriteLines) { }
}

public class TankSprite : Sprite<MapTank> {
  private static readonly string[] SpriteDown = {
    "#v#",
    "#@#",
    "#║#"
  };
  
  private static readonly string[] SpriteUp = {
    "#║#",
    "#@#",
    "#^#"
  };
  
  private static readonly string[] SpriteLeft = {
    "###",
    "═@<",
    "###"
  };
  
  private static readonly string[] SpriteRight = {
    "###",
    ">@═",
    "###"
  };
    
  // 
  public TankSprite() : base(SpriteDown) { }

  public override char[,] GetSprite(MapTank tank) {
    string[] initString = tank.Dir switch {
      Direction.Up => SpriteUp,
      Direction.Down => SpriteDown,
      Direction.Left => SpriteLeft,
      Direction.Right => SpriteRight,
      _ => SpriteDown
    };
    
    InitChars(initString);
    
    return Chars;
  }
}

public class UnknownSprite : Sprite<MapObject> {
  private static readonly string[] SpriteLines = {
    "???",
    "???",
    "???"
  };
    
  public UnknownSprite() : base(SpriteLines) { }
}
