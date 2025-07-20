using AsciiBlitz;
using AsciiBlitz.Debug;
using AsciiBlitz.Game;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// DebugUtils.PrintAInAllColors(1);
// Console.ReadKey();

// string[] maze = MazeGenerator.GenerateValidMaze(30, 20, 10, 123);
// MazeGenerator.DrawColoredMaze(maze);

try {
  GameRunner gameRunner = new GameRunner();
  gameRunner.Run();
} catch (Exception e) {
  Console.WriteLine(e);
}
