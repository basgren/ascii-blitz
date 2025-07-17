using AsciiBlitz;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// DebugUtils.PrintAInAllColors(1);
// Console.ReadKey();
try {
  GameRunner gameRunner = new GameRunner();
  gameRunner.Run();
} catch (Exception e) {
  Console.WriteLine(e);
}
