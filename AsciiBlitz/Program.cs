using AsciiBlitz;
using AsciiBlitz.Debug;
using AsciiBlitz.Game;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// DebugUtils.PrintAInAllColors(1);
// Console.ReadKey();

try {
  GameRunner gameRunner = new GameRunner();
  gameRunner.Run();
} catch (Exception e) {
  Console.WriteLine(e);
}
