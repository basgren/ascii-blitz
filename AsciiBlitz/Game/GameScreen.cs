using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Game.Objects.Tank;

namespace AsciiBlitz.Game;

public class GameScreen {
  private const int MenuWidth = 21;
  private const int Gap = 1;
  private readonly GameViewportRenderer _mapRenderer = new();

  private ScreenBuffer _gameViewport = new(0, 0);
  private ScreenBuffer _menu = new(0, 0);
  private ScreenBuffer _bulletViewport = new(15, 3);

  private int _screenWidth;
  private int _screenHeight;
  private DrawUtils _draw;

  public GameScreen() {
    _draw = new DrawUtils(_menu);
  }

  public void Render(ScreenBuffer target, IGameMap map, Tank player, IFrameContext frame) {
    InitSurfaces(target);

    _mapRenderer
      .SetCameraCenterWorldCoords(player.Pos)
      .Render(_gameViewport, map, frame.ElapsedTime);

    DrawInfoPanel(player, frame);

    _draw
      .SetTarget(target)
      .DrawBorder(0, 0, _gameViewport.Width + 2, _gameViewport.Height + 2, DrawUtils.Rounded)
      .DrawBorder(_screenWidth - MenuWidth - Gap * 2, 0, MenuWidth + 2, _screenHeight, DrawUtils.DoubleLine);

    target.DrawFrom(_gameViewport, 1, 1);
    target.DrawFrom(_menu, _screenWidth - MenuWidth - Gap, 1);
  }

  private void DrawInfoPanel(Tank player, IFrameContext frame) {
    _draw
      .SetTarget(_menu)
      .DrawTextLines(1, 0, HeaderLines);

    int weapX = 2;
    int weapY = 7;
    int weapWidth = 15;

    _draw
      .DrawText(weapX + 5, weapY, "Weapon")
      .SetColor(
        player.WeaponState.State == TankWeaponState.Idle
          ? AnsiColor.Green
          : AnsiColor.Grayscale(10)
      )
      .DrawBorder(weapX, weapY + 1, weapWidth + 2, 5, DrawUtils.HeavyLine)
      .ResetColor();

    int baseBulletX = 2;

    int bulletX = player.WeaponState.State switch {
      TankWeaponState.Shooting => baseBulletX + (int)(player.WeaponState.Progress * 15),
      TankWeaponState.Reloading => -20 + (int)(player.WeaponState.Progress * 20) + baseBulletX,
      _ => baseBulletX,
    };

    int bulletColor = player.WeaponState.State switch {
      TankWeaponState.Shooting => AnsiColor.BrightRed,
      TankWeaponState.Reloading => AnsiColor.Grayscale(10),
      _ => AnsiColor.BrightWhite,
    };

    _draw
      .SetTarget(_bulletViewport)
      .Clear()
      .SetColor(bulletColor)
      .DrawTextLines(bulletX, 0, BulletLines)
      .ResetColor();

    _menu.DrawFrom(_bulletViewport, weapX + 1, weapY + 2);

    string[] menuLines = [
      "\u2190\u2192 Turn   \u2191\u2193 Move",
      "   [space] Fire",
      "",
      "1 - Test map",
      "Esc - Quit",
    ];
    
    var dbgMenuY = _menu.Height - menuLines.Length - 3;
    
    _draw
      .SetTarget(_menu)
      .DrawTextLines(1, dbgMenuY, menuLines)
      .SetColor(AnsiColor.Grayscale(8))
      .DrawText(1, _menu.Height - 3, $"FProc: {(frame.FrameProcessingTime * 1000):F2}ms")
      .DrawText(1, _menu.Height - 2, $"FPS: {frame.FPS:F2}")
      .DrawText(1, _menu.Height - 1, $"Pos: {player.Pos.X:F2}; {player.Pos.Y:F2}")
      .ResetColor();
  }

  private void InitSurfaces(ScreenBuffer target) {
    if (_screenWidth == target.Width || _screenHeight == target.Height) {
      _gameViewport.Clear();
      _menu.Clear();
      return;
    }

    _screenWidth = target.Width;
    _screenHeight = target.Height;

    _gameViewport.SetSize(target.Width - MenuWidth - Gap * 4, target.Height - Gap * 2);
    // _gameViewport.SetSize(40, 20);

    _menu.SetSize(MenuWidth, target.Height - Gap * 2);
  }

  // Thanks to Patrick Gillespie: https://patorjk.com/software/taag/#p=display&f=Rectangles&t=Ascii%0ABlitz
  private static readonly string[] HeaderLines = [
    "     A S C I I     ",
    " _____ _ _ _       ",
    "| __  | |_| |_ ___ ",
    "| __ -| | |  _|- _|",
    "|_____|_|_|_| |___|",
  ];

  private static readonly string[] BulletLines = [
    @"/=====__",
    @"|########>",
    @"\====="""""
  ];
}
