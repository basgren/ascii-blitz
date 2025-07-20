namespace AsciiBlitz.Core.Map.Generator;

public static class MazeBuilder {
  private static Random _rand = new();

  public static void SetSeed(int seed) {
    _rand = new Random(seed);
  }

  public static char[,] GenerateMaze(int width, int height) {
    if (width % 2 == 0) {
      width++;
    }

    if (height % 2 == 0) {
      height++;
    }

    var maze = new char[height, width];

    for (int y = 0; y < height; y++) {
      for (int x = 0; x < width; x++) {
        maze[y, x] = MapSymbols.Wall;
      }
    }

    var visited = new bool[height, width];

    void Carve(int x, int y) {
      visited[y, x] = true;
      maze[y, x] = MapSymbols.Empty;

      foreach (var (dx, dy) in Shuffle(new[] { (0, -2), (2, 0), (0, 2), (-2, 0) })) {
        int nx = x + dx, ny = y + dy;

        if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 && !visited[ny, nx]) {
          maze[y + dy / 2, x + dx / 2] = MapSymbols.Empty;
          Carve(nx, ny);
        }
      }
    }

    Carve(1, 1);

    // Add destructible walls
    for (int y = 1; y < height - 1; y++) {
      for (int x = 1; x < width - 1; x++) {
        if (maze[y, x] == MapSymbols.Wall && _rand.NextSingle() < 0.2) {
          maze[y, x] = MapSymbols.Destructible;
        }
      }
    }

    PlaceRiver(maze);
    PlacePatches(maze, MapSymbols.Grass, 8, (3, 3), (6, 6));
    PlacePatches(maze, MapSymbols.Wheat, 6, (3, 3), (5, 6));
    PlacePlayerAndExit(maze);
    PlaceEnemies(maze, 3);

    return maze;
  }

  private static void SetChar(char[,] maze, int x, int y, char c) {
    if (x < 0 || y < 0 || x >= maze.GetLength(1) || y >= maze.GetLength(0)) {
      return;
    }
    
    maze[y, x] = c; 
  }

  private static void PlaceRiver(char[,] maze) {
    int height = maze.GetLength(0), w = maze.GetLength(1);
    int riverWidth = _rand.Next(2, 5);
    int x = _rand.Next(w / 3, w * 2 / 3);
    List<(int x1, int x2)> cols = new();

    for (int y = 0; y < height; y++) {
      x = Math.Clamp(x + _rand.Next(-1, 2), 1, w - riverWidth - 1);
      
      for (int i = 0; i < riverWidth; i++) {
        maze[y, x + i] = MapSymbols.River;
      }
      
      // Grass should be along the river.
      SetChar(maze, x - 1, y, MapSymbols.Grass);
      SetChar(maze, x + riverWidth, y, MapSymbols.Grass);

      cols.Add((x, x + riverWidth - 1));
    }

    var usedRows = new HashSet<int>();
    for (int i = 0; i < 3; i++) {
      int ry = _rand.Next(1, height - 1);
     
      if (usedRows.Contains(ry)) {
        continue;
      }

      for (int rx = cols[ry].x1; rx <= cols[ry].x2; rx++) {
        maze[ry, rx] = MapSymbols.Bridge;
      }

      usedRows.Add(ry);
    }
  }

  private static void PlacePatches(char[,] maze, char symbol, int count, (int, int) min, (int, int) max) {
    int h = maze.GetLength(0), w = maze.GetLength(1);
    for (int i = 0; i < count; i++) {
      int ph = _rand.Next(min.Item1, max.Item1 + 1);
      int pw = _rand.Next(min.Item2, max.Item2 + 1);
      int y = _rand.Next(1, h - ph - 1);
      int x = _rand.Next(1, w - pw - 1);

      for (int dy = 0; dy < ph; dy++) {
        for (int dx = 0; dx < pw; dx++) {
          if (maze[y + dy, x + dx] == MapSymbols.Empty) {
            maze[y + dy, x + dx] = symbol;
          }
        }
      }
    }
  }

  private static void PlacePlayerAndExit(char[,] maze) {
    int h = maze.GetLength(0), w = maze.GetLength(1);
    for (int x = 1; x < w - 1; x++) {
      if (maze[1, x] == MapSymbols.Empty) {
        maze[1, x] = MapSymbols.Player;
        break;
      }
    }

    for (int x = w - 2; x > 0; x--) {
      if (maze[h - 2, x] == MapSymbols.Empty) {
        maze[h - 2, x] = MapSymbols.Exit;
        break;
      }
    }
  }

  private static void PlaceEnemies(char[,] maze, int count) {
    int h = maze.GetLength(0), w = maze.GetLength(1);
    int placed = 0;
    
    while (placed < count) {
      int x = _rand.Next(1, w - 1);
      int y = _rand.Next(1, h - 1);
    
      if (maze[y, x] == MapSymbols.Empty) {
        maze[y, x] = MapSymbols.Enemy;
        placed++;
      }
    }
  }

  private static IEnumerable<(int, int)> Shuffle((int, int)[] array) {
    return array.OrderBy(_ => _rand.Next());
  }
}
