namespace AsciiBlitz.Game.Map;

public class InteractiveMapGenerator(int startWidth, int startHeight, int seed) {
  private MazeGenerationOptions _options = new(MakeOdd(startWidth), MakeOdd(startHeight), seed); 

  private const int MinWidth = 11;
  private const int MaxWidth = 49;
  private const int MinHeight = 11;
  private const int MaxHeight = 25; // Limit just to make everything fit window height.
  private const int MinSeed = 1;

  public MazeGenerationOptions ReadOptions() {
    ConsoleKey key;
    string[] maze = null!;

    do {
      Console.Clear();
      maze = MazeGenerator.GenerateValidMaze(_options, 10);
      Console.WriteLine("Ascii Blitz - Interactive Map Generator");
      Console.WriteLine("[←→] Width  [↑↓] Height  [Q/A] Seed  [R]andom  [Enter] Confirm  [Esc] Exit");
      Console.WriteLine($"Width: {_options.Width}, Height: {_options.Height}, Seed: {_options.Seed}");
      DrawColoredMaze(maze);
      key = Console.ReadKey(true).Key;

      switch (key) {
        case ConsoleKey.LeftArrow:
          _options.Width = Math.Max(MinWidth, _options.Width - 2);
          break;
        case ConsoleKey.RightArrow:
          _options.Width = Math.Min(MaxWidth, _options.Width + 2);
          break;
        case ConsoleKey.UpArrow:
          _options.Height = Math.Min(MaxHeight, _options.Height + 2);
          break;
        case ConsoleKey.DownArrow:
          _options.Height = Math.Max(MinHeight, _options.Height - 2);
          break;
        case ConsoleKey.Q:
          _options.Seed++;
          break;
        case ConsoleKey.A:
          _options.Seed = Math.Max(MinSeed, _options.Seed - 1);
          break;
        case ConsoleKey.R:
          var rng = new Random();
          _options.Width = MakeOdd(rng.Next(MinWidth, MaxWidth + 1));
          _options.Height = MakeOdd(rng.Next(MinHeight, MaxHeight + 1));
          _options.Seed = rng.Next(MinSeed, 10000);
          break;
        
        case ConsoleKey.Enter:
          return _options;
        
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
    MapSymbols.WeakWall => ConsoleColor.Gray,
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
    MapSymbols.WeakWall => ConsoleColor.Gray,
    MapSymbols.Grass => ConsoleColor.Green,
    MapSymbols.Wheat => ConsoleColor.Yellow,
    _ => ConsoleColor.Black,
  };
}
