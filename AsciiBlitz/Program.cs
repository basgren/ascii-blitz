// See https://aka.ms/new-console-template for more information

using AsciiBlitz.Core;
using AsciiBlitz.Debug;

Console.OutputEncoding = System.Text.Encoding.UTF8;

bool running = true;
int perRow = 24;

// DebugUtils.PrintAInAllColors();

Game game = new Game();
game.Run();
