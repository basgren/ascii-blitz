using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Game.Map;

namespace AsciiBlitz.Game.Rooms.MapGen;

public class MapGenContext {
  public MazeGenerationOptions Options { get; set; } = new(11, 11, 1);
  public string[]? Map { get; set; }
  public int OffsX { get; set; }
  public int OffsY { get; set; }
}

public class MapGenScreen {
  public ScreenBuffer GameViewport => _gameViewport;
  public ScreenBuffer MapBuffer => _mapBuffer;
  
  private const int MenuWidth = 21;
  private const int Gap = 1;

  private readonly ScreenBuffer _gameViewport = new(0, 0);
  private readonly ScreenBuffer _mapBuffer = new(15, 3);
  private readonly ScreenBuffer _menu = new(0, 0);
  private readonly string[]? _renderedMap;

  private int _screenWidth;
  private int _screenHeight;
  private readonly DrawUtils _draw;

  public MapGenScreen() {
    _draw = new DrawUtils(_menu);
  }

  public void Render(ScreenBuffer target, MapGenContext mapGenContext, IFrameContext frame) {
    InitSurfaces(target);

    DrawInfoPanel(mapGenContext.Options, frame);
    RenderMap(_mapBuffer, mapGenContext.Map);
    
    _draw
      .SetTarget(target)
      .DrawRect(0, 0, _gameViewport.Width + 2, _gameViewport.Height + 2, DrawUtils.Rounded)
      .DrawRect(_screenWidth - MenuWidth - Gap * 2, 0, MenuWidth + 2, _screenHeight, DrawUtils.DoubleLine);

    int x = _gameViewport.Width / 2 - _mapBuffer.Width / 2 + mapGenContext.OffsX;
    int y = _gameViewport.Height / 2 - _mapBuffer.Height / 2 + mapGenContext.OffsY;
    
    _gameViewport.DrawFrom(_mapBuffer, x, y);
    
    target.DrawFrom(_gameViewport, 1, 1);
    target.DrawFrom(_menu, _screenWidth - MenuWidth - Gap, 1);
  }

  private void RenderMap(ScreenBuffer mapBuffer, string[] map) {
    if (_renderedMap == map) {
      return;
    }
    
    var height = map.Length;
    var width = map[0].Length;
    
    mapBuffer.SetSize(width * 2, height);

    for (int y = 0; y < height; y++) {
      for (int x = 0; x < width; x++) {
        ScreenCell cell = GetMapCell(map[y][x]);
        mapBuffer.Set(x * 2, y, cell);
        mapBuffer.Set(x * 2 + 1, y, cell);
      }
    }
    
  }

  private ScreenCell GetMapCell(char c) {
    var color = GetCharColor(c);
    return new ScreenCell(c, color, color);
  }
  
  private static int GetCharColor(char c) {
    return c switch {
      MapSymbols.Wall => AnsiColor.Grayscale(15),
      MapSymbols.WeakWall => AnsiColor.Grayscale(18),
      MapSymbols.Grass => AnsiColor.Green,
      MapSymbols.Wheat => AnsiColor.BrightYellow,
      MapSymbols.River => AnsiColor.Blue,
      MapSymbols.Bridge => AnsiColor.Rgb(2, 1, 0),
      MapSymbols.Player => AnsiColor.Cyan,
      MapSymbols.Exit => AnsiColor.Black, // Don't display exit for now.  AnsiColor.Magenta,
      MapSymbols.Enemy => AnsiColor.Red,
      _ => AnsiColor.Black,
    };
  }

  private void DrawInfoPanel(MazeGenerationOptions options, IFrameContext frame) {
    _draw
      .SetTarget(_menu)
      .DrawTextLines(1, 0, HeaderLines)
      .DrawTextCentered(_menu.Width / 2, HeaderLines.Length + 5, $"Seed: {options.Seed}")
      .DrawTextCentered(_menu.Width / 2, HeaderLines.Length + 6, $"Size: {options.Width}x{options.Height}");
    
    string[] menuLines = [
      "[←→↑↓] Panning",
      "[A/D] Width",
      "[W/S] Height",
      "[Q/E] Seed",
      "[R] Random",
      "[Enter] Play",
      "[Esc] Main Menu",
    ];
    
    var dbgMenuY = _menu.Height - menuLines.Length - 4;
    
    _draw
      .SetTarget(_menu)
      .DrawTextLines(1, dbgMenuY, menuLines)
      .SetColor(AnsiColor.Grayscale(8))
      .DrawText(1, _menu.Height - 3, $"FProc: {(frame.FrameProcessingTime * 1000):F2}ms")
      .DrawText(1, _menu.Height - 2, $"FPS: {frame.FPS:F2}")
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
}
