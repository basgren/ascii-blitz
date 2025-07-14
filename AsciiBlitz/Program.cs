// See https://aka.ms/new-console-template for more information

using AsciiBlitz;
using AsciiBlitz.Core;
using AsciiBlitz.Debug;

ConsoleUtils.SetConsoleSize(120, 40);
Console.OutputEncoding = System.Text.Encoding.UTF8;

// DebugUtils.PrintAInAllColors(1);
// Console.ReadKey();

Game game = new Game();
game.Run();
