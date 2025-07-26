using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Render.Sprites;
using AsciiBlitz.Core.Types;

using static AsciiBlitz.Core.Render.AnsiColor;

namespace AsciiBlitz.Game.Objects.Tank;

public class TankSprite() : Sprite(5, 3) {
  private static readonly string[] SpriteDown = [
    "#---#",
    "#(@)#",
    "#=║=#",
  ];

  private static readonly string[] SpriteUp = [
    "#=║=#",
    "#(@)#",
    "#---#",
  ];
  
  private static readonly string[] SpriteLeft = [
    "#####",
    "═(@)|",
    "#####",
  ];
  
  private static readonly string[] SpriteRight = [
    "#####",
    "|(@)=",
    "#####",
  ];

  private static readonly int TracksGrayscaleId = 12;
  private static readonly int TracksBgColor = Grayscale(5);
  private static readonly int HullColor = Rgb(0, 4,0);
  private static readonly int HullBgColor = Rgb(0, 1,0);
  
  private static readonly int EnemyHullColor = Rgb(4,0, 0);
  private static readonly int EnemyHullBgColor = Rgb(1, 0, 0);

  public override void UpdateCell(in CharContext context, ref ScreenCell cell) {
    if (context.GameObject is not Tank tank) {
      return;
    }
    
    var x = context.CharPos.X;
    var y = context.CharPos.Y;
    
    string[] charInfo = tank.Dir switch {
      Direction.Up => SpriteUp,
      Direction.Down => SpriteDown,
      Direction.Left => SpriteLeft,
      Direction.Right => SpriteRight,
      _ => SpriteDown
    };
    
    cell.Char = charInfo[y][x];
    
    var isHit = tank.DamageState.State == TankDamageState.Hit;

    if (IsHullChar(x, y, tank.Dir)) {
      if (tank.IsPlayer) {
        cell.Color = HullColor;
        cell.BgColor = HullBgColor;
      } else {
        cell.Color = EnemyHullColor;
        cell.BgColor = EnemyHullBgColor;
      }
    } else {
      if (tank.MovementState.State != TankMovementState.Idle) {
        int sign = tank.Dir is Direction.Right or Direction.Down ? -1 : 1;
        int offset = (int)(Math.Sin((sign * (x + y) + context.GameTime * 10)) * 3f);
        cell.Color = Grayscale(TracksGrayscaleId + offset);
      } else {
        cell.Color = Grayscale(TracksGrayscaleId);        
      }
      
      cell.BgColor = TracksBgColor;
    }
    
    if (isHit) {
      cell.BgColor = Grayscale((int)(6 + 17 * (1 - tank.DamageState.Progress)));
    }
    
    // Debug health. Uncomment to show HP on tank tower
    if (x == 2 && y == 1) {
      int health = (int)tank.Damageable.Health;
      cell.Char = (char)('0' + health);
    }
  }

  private bool IsHullChar(int x, int y, Direction dir) {
    if (dir is Direction.Down or Direction.Up) {
      return x != 0 && x != Width - 1;
    }
    
    return y != 0 && y != Height - 1;
  }
}
