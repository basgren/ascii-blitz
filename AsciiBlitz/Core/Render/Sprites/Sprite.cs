using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Render.Sprites;

public abstract class Sprite {
  public abstract char[,] GetChars(MapObject mapObject);
  public abstract char[,]? GetColors(MapObject mapObject);
}

// Color coding:
// l = bLack
// r = Red
// g = Green
// y = Yellow
// b = Blue
// p = Purple
// c = Cyan
// w = White

public abstract class Sprite<T> : Sprite where T: MapObject {
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
  
  protected void InitColors(string[] lines) {
    for (int y = 0; y < 3; y++) {
      var line = lines[y].PadRight(3);
      
      for (int x = 0; x < 3; x++) {
        Colors[x, y] = line[x];
      }
    }
  }

  public override char[,] GetChars(MapObject mapObject) {
    if (mapObject is T typedObject) {
      return GetChars(typedObject);
    }

    return Chars;
  }

  public override char[,]? GetColors(MapObject mapObject) {
    if (mapObject is T typedObject) {
      return GetColors(typedObject);
    }
    
    return null;
  }

  protected virtual char[,] GetChars(T mapObject) {
    return Chars;
  }

  protected virtual char[,]? GetColors(T mapObject) {
    return null;
  }
}

public class EmptySprite : Sprite<MapEmpty> {
  private static readonly string[] SpriteLines = [
    "   ",
    "   ",
    "   "
  ];
    
  public EmptySprite() : base(SpriteLines) { }
}

public class WallSprite : Sprite<MapWall> {
  private static readonly string[] SpriteLines = [
    "███",
    "███",
    "███"
  ];
    
  public WallSprite() : base(SpriteLines) { }
}

public class GrassSprite : Sprite<MapGrass> {
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
    "YVWvw,.'   ",
    "ywv;,.'  ",
    "vy.,.'  ",
    "v,.`    ",
    "#.`    ",
    "#.       ",
  ];

  protected override char[,] GetChars(MapGrass mapObject) {
    for (int y = 0; y < SpriteHeight; y++) {
      for (int x = 0; x < SpriteWidth; x++) {
        var samples = _grassChars[Math.Min(mapObject.GrassDamageLevel, _grassChars.Length - 1)];
        int index = randInt((x + y * SpriteHeight) * mapObject.Id, samples.Length - 1);
        
        Chars[x, y] = samples[index];
      }
    }
    
    return Chars;
  }

  protected override char[,] GetColors(MapGrass grass) {
    if (grass.GrassDamageLevel >= 4) {
      InitColors(SpriteColors2);
    } else {
      InitColors(SpriteColors);
    }
    
    return Colors;
  }

  private int randInt(int value, int max) {
    double v = 12345 * (Math.Sin(value * 45678) * 0.5 + 0.5);

    return (int)Math.Floor((v - Math.Truncate(v)) * max);
  }
}

public class TankSprite : Sprite<MapTank> {
  private static readonly string[] SpriteDown = [
    "#v#",
    "#@#",
    "#║#"
  ];

  private static readonly string[] SpriteDownColor = [
    "wGw",
    "wGw",
    "wGw"
  ];
  
  private static readonly string[] SpriteUp = [
    "#║#",
    "#@#",
    "#^#"
  ];
  
  private static readonly string[] SpriteUpColor = [
    "wGw",
    "wGw",
    "wGw"
  ];
  
  private static readonly string[] SpriteLeft = [
    "###",
    "═@<",
    "###"
  ];
  
  private static readonly string[] SpriteLeftColor = [
    "www",
    "GGG",
    "www"
  ];
       
  private static readonly string[] SpriteRight = [
    "###",
    ">@═",
    "###"
  ];
  
  private static readonly string[] SpriteRightColor = [
    "www",
    "GGG",
    "www"
  ];
    
  // 
  public TankSprite() : base(SpriteDown) { }

  protected override char[,] GetChars(MapTank tank) {
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

  protected override char[,]? GetColors(MapTank tank) {
    string[] initString = tank.Dir switch {
      Direction.Up => SpriteUpColor,
      Direction.Down => SpriteDownColor,
      Direction.Left => SpriteLeftColor,
      Direction.Right => SpriteRightColor,
      _ => throw new ArgumentOutOfRangeException()
    };
    
    InitColors(initString);
    
    return Colors;
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
