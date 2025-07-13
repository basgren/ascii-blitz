using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Generator;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core;

public class Game {

  private GameMap _map;
  
  public void Run() {
    Console.Clear();
    Console.CursorVisible = false;
    Console.WriteLine("ASCII Blitz Game");


    IMapGenerator mapGen = new TestMapGenerator();
    
    _map = mapGen
      .SetSize(10, 10)
      .Build();

    // Test rendering - when needed to show generated map.
    // GameMapRenderer mapRenderer = new GameMapRenderer();
    // mapRenderer.Render(map);
    
    MapGridRenderer mapRenderer = new();
    
    // Main game loop - 60 FPS
    const int targetFps = 60;
    const int frameTimeMs = 1000 / targetFps;
    
    bool gameRunning = true;
    var lastFrameTime = DateTime.Now;
    
    while (gameRunning) {
      var currentTime = DateTime.Now;
      var deltaTime = (currentTime - lastFrameTime).TotalMilliseconds;
      
      // Handle input
      ConsoleKeyInfo keyInfo = default;
      bool keyPressed = false;
      
      if (Console.KeyAvailable) {
        keyInfo = Console.ReadKey(true);
        keyPressed = true;
        
        // Check for ESC to exit
        if (keyInfo.Key == ConsoleKey.Escape) {
          gameRunning = false;
          continue;
        }
      }
      
      mapRenderer.Render(_map);
      
      // Display key information at position 0,0
      Console.SetCursorPosition(0, 0);
      
      if (keyPressed) {
        // Display keycode for captured keys
        switch (keyInfo.Key) {
          case ConsoleKey.UpArrow:
            TryMove(Direction.Up);
            break;
          
          case ConsoleKey.DownArrow:
            TryMove(Direction.Down);
            break;
          
          case ConsoleKey.LeftArrow:
            TryMove(Direction.Left);
            break;

          case ConsoleKey.RightArrow:
            TryMove(Direction.Right);
            break;
          
          case ConsoleKey.Spacebar:
          case ConsoleKey.C:
            // Console.Write($"Key: {(int)keyInfo.Key}    ");
            break;
          default:
            // Console.Write("Key: 0   ");
            break;
        }
      } else {
        // No key pressed
        // Console.Write("Key: 0   ");
      }
      
      // Frame rate limiting
      if (deltaTime < frameTimeMs) {
        Thread.Sleep((int)(frameTimeMs - deltaTime));
      }
      
      lastFrameTime = DateTime.Now;
    }
  }

  private void TryMove(Direction dir) {
    var layer = _map.GetUnitLayer();
    Vec2Int offs = new Vec2Int(0, 0);
    
    switch (dir) {
      case Direction.Up:
        offs = Vec2Int.Up;
        break;

      case Direction.Down:
        offs = Vec2Int.Down;
        break;
      
      case Direction.Left:
        offs = Vec2Int.Left;
        break;
      
      case Direction.Right:
        offs = Vec2Int.Right;
        break;
    }
    
    var player = layer.Player;
    player.Dir = dir;

    if (_map.CanMove(player.Pos, offs)) {
      player.Pos += offs;
      
      MapObject? obj = _map.GetLayer(0).GetAt(player.Pos.X, player.Pos.Y);

      if (obj != null) {
        obj.Visited();
      }
    }
  }
}
