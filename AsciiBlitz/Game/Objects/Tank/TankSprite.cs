using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Types;

using static AsciiBlitz.Core.Render.AnsiColor;

namespace AsciiBlitz.Game.Objects.Tank;

public class TankSprite() : Sprite(3, 3) {
  private static readonly string[] SpriteDown = [
    "#v#",
    "#@#",
    "#║#",
  ];

  private static readonly int[,] SpriteDownColors = {
    { White, Green, White },
    { White, Green, White },
    { White, Green, White },
  };
  
  private static readonly string[] SpriteUp = [
    "#║#",
    "#@#",
    "#^#",
  ];
  
  private static readonly string[] SpriteUpColor = [
    "wGw",
    "wGw",
    "wGw",
  ];
  
  private static readonly int[,] SpriteUpColors = {
    { White, Green, White },
    { White, Green, White },
    { White, Green, White },
  };
  
  private static readonly string[] SpriteLeft = [
    "###",
    "═@<",
    "###",
  ];
  
  private static readonly int[,] SpriteLeftColors = {
    { White, White, White },
    { Green, Green, Green },
    { White, White, White },
  };
       
  private static readonly string[] SpriteRight = [
    "###",
    ">@═",
    "###",
  ];
  
  private static readonly int[,] SpriteRightColors = {
    { White, White, White },
    { Green, Green, Green },
    { White, White, White },
  };
  
  public override void UpdateCell(in SpriteContext context, ref ScreenCell cell) {
    if (context.GameObject is not Tank tank) {
      return;
    }
    
    string[] charInfo = tank.Dir switch {
      Direction.Up => SpriteUp,
      Direction.Down => SpriteDown,
      Direction.Left => SpriteLeft,
      Direction.Right => SpriteRight,
      _ => SpriteDown
    };
    
    int[,] colorInfo = tank.Dir switch {
      Direction.Up => SpriteUpColors,
      Direction.Down => SpriteDownColors,
      Direction.Left => SpriteLeftColors,
      Direction.Right => SpriteRightColors,
      _ => throw new ArgumentOutOfRangeException()
    };

    cell.Char = charInfo[context.CharPos.Y][context.CharPos.X];
    cell.Color = colorInfo[context.CharPos.Y, context.CharPos.X];
  }
}
