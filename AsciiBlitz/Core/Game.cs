using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Generator;
using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Core;

public class Game {
  public void Run() {
    Console.Clear();
    Console.CursorVisible = false;
    Console.WriteLine("ASCII Blitz Game");


    IMapGenerator mapGen = new TestMapGenerator();
    
    GameMap map = mapGen
      .SetSize(10, 10)
      .Build();

    // Test rendering - when needed to show generated map.
    // GameMapRenderer mapRenderer = new GameMapRenderer();
    // mapRenderer.Render(map);
    
    MapGridRenderer mapRenderer = new();
    mapRenderer.Render(map);
    
    Console.ReadKey();
  }
}
