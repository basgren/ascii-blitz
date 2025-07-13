using System.Drawing;

using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Core.Render;

public class MapGridRenderer {
  private const int CellWidth = 3;
  private const int CellHeight = 3;

  private readonly Dictionary<MapObjectType, Sprite> _spriteMapping;
  private readonly UnknownSprite _unknownSprite;

  public MapGridRenderer() {
    _spriteMapping = new Dictionary<MapObjectType, Sprite> {
      { MapObjectType.Empty, new EmptySprite() },
      { MapObjectType.Wall, new WallSprite() },
      { MapObjectType.Tank, new TankSprite() },
      { MapObjectType.Grass, new GrassSprite() },
    };

    _unknownSprite = new UnknownSprite();
  }

  public void Render(GameMap map, double timeSeconds) {
    // Get console size to check available space
    int consoleWidth = Console.WindowWidth;
    int consoleHeight = Console.WindowHeight;

    // Calculate how many map cells we can fit (each cell is 3x3)
    int maxMapWidth = consoleWidth / 3;
    int maxMapHeight = consoleHeight / 3;

    // Determine the actual render area
    int renderWidth = Math.Min(map.Width, maxMapWidth);
    int renderHeight = Math.Min(map.Height, maxMapHeight);

    // Render the map
    for (int mapY = 0; mapY < renderHeight; mapY++) {
      for (int mapX = 0; mapX < renderWidth; mapX++) {
        MapObject mapObject = GetMapObject(map, mapX, mapY) ?? MapEmpty.Instance;
        var sprite = GetSpriteForMapObject(mapObject);
        var spriteData = sprite.GetChars(mapObject, timeSeconds);
        var colors = sprite.GetColors(mapObject, timeSeconds);

        for (int spriteY = 0; spriteY < CellHeight; spriteY++) {
          // Render 3 characters for each map cell
          for (int spriteX = 0; spriteX < CellWidth; spriteX++) {
            var x = mapX * CellWidth + spriteX;
            var y = mapY * CellHeight + spriteY;
            if (x >= consoleWidth || y >= consoleHeight) {
              continue;
            }

            string sym = colors != null
              ? ApplyColor(spriteData[spriteX, spriteY], colors[spriteX, spriteY])
              : spriteData[spriteX, spriteY].ToString();

            Console.SetCursorPosition(x, y);
            Console.Write(sym);
          }
        }
      }
    }
  }

  private string ApplyColor(char c, char color) {
    string s = c.ToString();

    return color switch {
      'l' => C.Black(s),
      'L' => C.BlackBold(s),
      'r' => C.Red(s),
      'R' => C.RedBold(s),
      'g' => C.Green(s),
      'G' => C.GreenBold(s),
      'y' => C.Yellow(s),
      'Y' => C.YellowBold(s),
      'b' => C.Blue(s),
      'B' => C.BlueBold(s),
      'p' => C.Purple(s),
      'P' => C.PurpleBold(s),
      'c' => C.Cyan(s),
      'C' => C.CyanBold(s),
      'w' => C.White(s),
      'W' => C.WhiteBold(s),
      'x' => C.Gray(s),
      _ => s, // Default to no color
    };
  }

  private MapObject? GetMapObject(GameMap map, int x, int y) {
    // Check layers from highest index to lowest (back to front)
    for (int layerIndex = map.LayerCount - 1; layerIndex >= 0; layerIndex--) {
      var layer = map.GetLayer(layerIndex);
      var mapObject = layer.GetAt(x, y);

      if (mapObject != null) {
        return mapObject;
      }
    }

    return null;
  }

  private Sprite GetSpriteForMapObject(MapObject mapObject) {
    if (_spriteMapping.TryGetValue(mapObject.Type, out var sprite)) {
      return sprite;
    }

    // Return unknown sprite for unmapped objects
    return _unknownSprite;
  }
}

/**
 * Class that simplifies work with ANSI colors.
 */
public static class C {
  // It's more convenient to use ANSI escape codes to colorize text, rather than use Console methods.
  // May not work on Windows in old cmd.exe and PowerShell older than 7.
  private const string AnsiBlack = "\e[0;30m";
  private const string AnsiBlackBold = "\e[1;30m";
  private const string AnsiRed = "\e[0;31m";
  private const string AnsiRedBold = "\e[1;31m";
  private const string AnsiGreen = "\e[0;32m";
  private const string AnsiGreenBold = "\e[1;32m";
  private const string AnsiYellow = "\e[0;33m";
  private const string AnsiYellowBold = "\e[1;33m";
  private const string AnsiBlue = "\e[0;34m";
  private const string AnsiBlueBold = "\e[1;34m";
  private const string AnsiPurple = "\e[0;35m";
  private const string AnsiPurpleBold = "\e[1;35m";
  private const string AnsiCyan = "\e[0;36m";
  private const string AnsiCyanBold = "\e[1;36m";
  private const string AnsiWhite = "\e[0;37m";
  private const string AnsiWhiteBold = "\e[1;37m";
  private const string AnsiGray = "\e[0;90m";

  private const string AnsiReset = "\x1b[0m";

  public static string Black(string s) => ResetAfter($"{AnsiBlack}{s}");
  public static string BlackBold(string s) => ResetAfter($"{AnsiBlackBold}{s}");
  public static string Red(string s) => ResetAfter($"{AnsiRed}{s}");
  public static string RedBold(string s) => ResetAfter($"{AnsiRedBold}{s}");
  public static string Green(string s) => ResetAfter($"{AnsiGreen}{s}");
  public static string GreenBold(string s) => ResetAfter($"{AnsiGreenBold}{s}");
  public static string Yellow(string s) => ResetAfter($"{AnsiYellow}{s}");
  public static string YellowBold(string s) => ResetAfter($"{AnsiYellowBold}{s}");
  public static string Blue(string s) => ResetAfter($"{AnsiBlue}{s}");
  public static string BlueBold(string s) => ResetAfter($"{AnsiBlueBold}{s}");
  public static string Purple(string s) => ResetAfter($"{AnsiPurple}{s}");
  public static string PurpleBold(string s) => ResetAfter($"{AnsiPurpleBold}{s}");
  public static string Cyan(string s) => ResetAfter($"{AnsiCyan}{s}");
  public static string CyanBold(string s) => ResetAfter($"{AnsiCyanBold}{s}");
  public static string Gray(string s) => ResetAfter($"{AnsiGray}{s}");
  public static string White(string s) => ResetAfter($"{AnsiWhite}{s}");
  public static string WhiteBold(string s) => ResetAfter($"{AnsiWhiteBold}{s}");

  private static string ResetAfter(string s) => $"{s}{AnsiReset}";
}
