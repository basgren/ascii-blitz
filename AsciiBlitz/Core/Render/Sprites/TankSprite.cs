using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Render.Sprites;

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

  protected override char[,] GetChars(MapTank tank, double timeSeconds) {
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

  protected override char[,]? GetColors(MapTank tank, double timeSeconds) {
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
