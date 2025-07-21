namespace AsciiBlitz.Game.Map;

public class InteractiveMapGenerator(int startWidth, int startHeight, int seed) {
  private int _width = MakeOdd(startWidth);
  private int _height = MakeOdd(startHeight);
  private int _seed = seed;

  private const int MinWidth = 11;
  private const int MaxWidth = 49;
  private const int MinHeight = 11;
  private const int MaxHeight = 25; // Limit just to make everything fit window height.
  private const int MinSeed = 1;

  public string[] Generate() {
    ConsoleKey key;
    string[] maze = null!;

    do {
      Console.Clear();
      maze = MazeGenerator.GenerateValidMaze(_width, _height, 10, _seed);
      Console.WriteLine("Ascii Blitz - Interactive Map Generator");
      Console.WriteLine("[←→] Width  [↑↓] Height  [Q/A] Seed  [R]andom  [Enter] Confirm  [Esc] Exit");
      Console.WriteLine($"Width: {_width}, Height: {_height}, Seed: {_seed}");
      DrawColoredMaze(maze);
      key = Console.ReadKey(true).Key;

      switch (key) {
        case ConsoleKey.LeftArrow:
          _width = Math.Max(MinWidth, _width - 2);
          break;
        case ConsoleKey.RightArrow:
          _width = Math.Min(MaxWidth, _width + 2);
          break;
        case ConsoleKey.UpArrow:
          _height = Math.Min(MaxHeight, _height + 2);
          break;
        case ConsoleKey.DownArrow:
          _height = Math.Max(MinHeight, _height - 2);
          break;
        case ConsoleKey.Q:
          _seed++;
          break;
        case ConsoleKey.A:
          _seed = Math.Max(MinSeed, _seed - 1);
          break;
        case ConsoleKey.R:
          var rng = new Random();
          _width = MakeOdd(rng.Next(MinWidth, MaxWidth + 1));
          _height = MakeOdd(rng.Next(MinHeight, MaxHeight + 1));
          _seed = rng.Next(MinSeed, 10000);
          break;
        case ConsoleKey.Enter:
          return maze;
        case ConsoleKey.Escape:
          Environment.Exit(0);
          break;
      }

    } while (true);
  }

  private static void DrawColoredMaze(string[] maze) {
    foreach (var line in maze) {
      foreach (var c in line) {
        Console.ForegroundColor = GetForeground(c);
        Console.BackgroundColor = GetBackground(c);
        Console.Write(c);
        Console.Write(c);
      }

      Console.WriteLine();
    }

    Console.ResetColor();
  }
  
  /// <summary>
  /// To avoid issues with generating walls, we have to provide odd numbers for width and height.
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  private static int MakeOdd(int value) => (value % 2 == 0) ? value + 1 : value;
  
  public static ConsoleColor GetForeground(char c) => c switch {
    MapSymbols.Wall => ConsoleColor.DarkGray,
    MapSymbols.Destructible => ConsoleColor.Gray,
    MapSymbols.Grass => ConsoleColor.Green,
    MapSymbols.Wheat => ConsoleColor.Yellow,
    MapSymbols.River => ConsoleColor.Blue,
    MapSymbols.Bridge => ConsoleColor.DarkYellow,
    MapSymbols.Player => ConsoleColor.Cyan,
    MapSymbols.Exit => ConsoleColor.Magenta,
    MapSymbols.Enemy => ConsoleColor.Red,
    _ => ConsoleColor.White,
  };

  public static ConsoleColor GetBackground(char c) => c switch {
    MapSymbols.River => ConsoleColor.Blue,
    MapSymbols.Bridge => ConsoleColor.DarkYellow,
    MapSymbols.Wall => ConsoleColor.DarkGray,
    MapSymbols.Destructible => ConsoleColor.Gray,
    MapSymbols.Grass => ConsoleColor.Green,
    MapSymbols.Wheat => ConsoleColor.Yellow,
    _ => ConsoleColor.Black,
  };
}
