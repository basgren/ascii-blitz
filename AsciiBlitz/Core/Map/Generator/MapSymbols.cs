namespace AsciiBlitz.Core.Map.Generator;

public static class MapSymbols {
  public const char Wall = '#';
  public const char Destructible = '%';
  public const char Empty = ' ';
  public const char Grass = 'g';
  public const char Wheat = 'w';
  public const char River = 'R';
  public const char Bridge = 'B';
  public const char Player = 'P';
  public const char Exit = 'E';
  public const char Enemy = 'T';

  public static readonly char[] Walkable = { Empty, Grass, Wheat, Bridge, Player, Exit, Enemy, Destructible };

  public static ConsoleColor GetForeground(char c) => c switch {
    Wall => ConsoleColor.DarkGray,
    Destructible => ConsoleColor.DarkYellow,
    Grass => ConsoleColor.Green,
    Wheat => ConsoleColor.Yellow,
    River => ConsoleColor.Blue,
    Bridge => ConsoleColor.Gray,
    Player => ConsoleColor.Cyan,
    Exit => ConsoleColor.Magenta,
    Enemy => ConsoleColor.Red,
    _ => ConsoleColor.White,
  };

  public static ConsoleColor GetBackground(char c) => ConsoleColor.Black;
}
