using AsciiBlitz.Core.Map.Generator;

namespace AsciiBlitz.Game;

public class MazeGenerator {
  public static string[] GenerateValidMaze(int width, int height, int maxAttempts = 10, int? seed = null) {
    if (seed.HasValue)
      MazeBuilder.SetSeed(seed.Value);

    for (int attempt = 0; attempt < maxAttempts; attempt++) {
      var maze = MazeBuilder.GenerateMaze(width, height);
      if (IsConnected(maze, out _, out _)) {
        return ConvertToStringArray(maze);
      }
    }

    throw new Exception("Failed to generate a connected maze.");
  }

  private static bool IsConnected(char[,] maze, out (int x, int y) playerPos, out (int x, int y) exitPos) {
    int height = maze.GetLength(0);
    int width = maze.GetLength(1);
    playerPos = (-1, -1);
    exitPos = (-1, -1);

    for (int y = 0; y < height; y++)
    for (int x = 0; x < width; x++) {
      if (maze[y, x] == MapSymbols.Player) playerPos = (x, y);
      if (maze[y, x] == MapSymbols.Exit) exitPos = (x, y);
    }

    if (playerPos == (-1, -1) || exitPos == (-1, -1))
      return false;

    var visited = new bool[height, width];
    var queue = new Queue<(int x, int y)>();
    queue.Enqueue(playerPos);
    visited[playerPos.y, playerPos.x] = true;

    while (queue.Count > 0) {
      var (x, y) = queue.Dequeue();
      if ((x, y) == exitPos)
        return true;

      foreach (var (nx, ny) in Neighbors(x, y, width, height)) {
        if (!visited[ny, nx] && MapSymbols.Walkable.Contains(maze[ny, nx])) {
          visited[ny, nx] = true;
          queue.Enqueue((nx, ny));
        }
      }
    }

    return false;
  }

  private static IEnumerable<(int x, int y)> Neighbors(int x, int y, int width, int height) {
    if (x > 0) yield return (x - 1, y);
    if (x < width - 1) yield return (x + 1, y);
    if (y > 0) yield return (x, y - 1);
    if (y < height - 1) yield return (x, y + 1);
  }

  private static string[] ConvertToStringArray(char[,] grid) {
    int height = grid.GetLength(0);
    int width = grid.GetLength(1);
    var result = new string[height];
    for (int y = 0; y < height; y++) {
      char[] row = new char[width];
      for (int x = 0; x < width; x++)
        row[x] = grid[y, x];
      result[y] = new string(row);
    }

    return result;
  }

  public static void DrawColoredMaze(string[] maze) {
    foreach (var line in maze) {
      foreach (var c in line) {
        Console.ForegroundColor = MapSymbols.GetForeground(c);
        Console.BackgroundColor = MapSymbols.GetBackground(c);
        Console.Write(c);
      }

      Console.WriteLine();
    }

    Console.ResetColor();
  }
}

public static class MazeBuilder {
  private static Random Rand = new();

  public static void SetSeed(int seed) {
    Rand = new Random(seed);
  }

  public static char[,] GenerateMaze(int width, int height) {
    if (width % 2 == 0) width++;
    if (height % 2 == 0) height++;

    var maze = new char[height, width];
    for (int y = 0; y < height; y++)
    for (int x = 0; x < width; x++)
      maze[y, x] = MapSymbols.Wall;

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
    for (int y = 1; y < height - 1; y++)
    for (int x = 1; x < width - 1; x++)
      if (maze[y, x] == MapSymbols.Wall && Rand.NextDouble() < 0.2)
        maze[y, x] = MapSymbols.Destructible;

    PlaceRiver(maze);
    PlacePatches(maze, MapSymbols.Grass, 8, (3, 3), (6, 6));
    PlacePatches(maze, MapSymbols.Wheat, 6, (3, 3), (5, 6));
    PlacePlayerAndExit(maze);
    PlaceEnemies(maze, 3);

    return maze;
  }

  private static void PlaceRiver(char[,] maze) {
    int h = maze.GetLength(0), w = maze.GetLength(1);
    int riverWidth = Rand.Next(2, 5);
    int x = Rand.Next(w / 3, w * 2 / 3);
    List<(int x1, int x2)> cols = new();

    for (int y = 0; y < h; y++) {
      x = Math.Clamp(x + Rand.Next(-1, 2), 1, w - riverWidth - 1);
      for (int i = 0; i < riverWidth; i++)
        maze[y, x + i] = MapSymbols.River;
      cols.Add((x, x + riverWidth - 1));
    }

    var usedRows = new HashSet<int>();
    for (int i = 0; i < 3; i++) {
      int ry = Rand.Next(1, h - 1);
      if (usedRows.Contains(ry)) continue;
      for (int rx = cols[ry].x1; rx <= cols[ry].x2; rx++)
        maze[ry, rx] = MapSymbols.Bridge;
      usedRows.Add(ry);
    }
  }

  private static void PlacePatches(char[,] maze, char symbol, int count, (int, int) min, (int, int) max) {
    int h = maze.GetLength(0), w = maze.GetLength(1);
    for (int i = 0; i < count; i++) {
      int ph = Rand.Next(min.Item1, max.Item1 + 1);
      int pw = Rand.Next(min.Item2, max.Item2 + 1);
      int y = Rand.Next(1, h - ph - 1);
      int x = Rand.Next(1, w - pw - 1);

      for (int dy = 0; dy < ph; dy++)
      for (int dx = 0; dx < pw; dx++)
        if (maze[y + dy, x + dx] == MapSymbols.Empty)
          maze[y + dy, x + dx] = symbol;
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
      int x = Rand.Next(1, w - 1);
      int y = Rand.Next(1, h - 1);
      if (maze[y, x] == MapSymbols.Empty) {
        maze[y, x] = MapSymbols.Enemy;
        placed++;
      }
    }
  }

  private static IEnumerable<(int, int)> Shuffle((int, int)[] array) {
    return array.OrderBy(_ => Rand.Next());
  }
}
