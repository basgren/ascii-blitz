namespace AsciiBlitz.Game.Map;

public struct MazeGenerationOptions(int width, int height, int? seed) {
  public int Width { get; set; } = width;
  public int Height { get; set; } = height;
  public int Seed { get; set; } = seed ?? 1;
}

public static class MazeGenerator {
  public static string[] GenerateValidMaze(MazeGenerationOptions attrs, int maxAttempts = 10) {
    MazeBuilder.SetSeed(attrs.Seed);

    for (int attempt = 0; attempt < maxAttempts; attempt++) {
      var maze = MazeBuilder.GenerateMaze(attrs.Width, attrs.Height);

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

    for (int y = 0; y < height; y++) {
      for (int x = 0; x < width; x++) {
        if (maze[y, x] == MapSymbols.Player) {
          playerPos = (x, y);
        }

        if (maze[y, x] == MapSymbols.Exit) {
          exitPos = (x, y);
        }
      }
    }

    if (playerPos == (-1, -1) || exitPos == (-1, -1)) {
      return false;
    }

    var visited = new bool[height, width];
    var queue = new Queue<(int x, int y)>();
    queue.Enqueue(playerPos);
    visited[playerPos.y, playerPos.x] = true;

    while (queue.Count > 0) {
      var (x, y) = queue.Dequeue();
      if ((x, y) == exitPos) {
        return true;
      }

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
    if (x > 0) {
      yield return (x - 1, y);
    }

    if (x < width - 1) {
      yield return (x + 1, y);
    }

    if (y > 0) {
      yield return (x, y - 1);
    }

    if (y < height - 1) {
      yield return (x, y + 1);
    }
  }

  private static string[] ConvertToStringArray(char[,] grid) {
    int height = grid.GetLength(0);
    int width = grid.GetLength(1);
    var result = new string[height];
    
    for (int y = 0; y < height; y++) {
      char[] row = new char[width];
      
      for (int x = 0; x < width; x++) {
        row[x] = grid[y, x];
      }

      result[y] = new string(row);
    }

    return result;
  }
}