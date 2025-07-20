using AsciiBlitz.Core;
using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Generator;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Game;

namespace AsciiBlitz;

public class GameRunner {
  private bool _gameRunning = true;
  private readonly GameState _gameState = new();

  private readonly BufferedConsoleRenderer _consoleRenderer = new();
  private readonly GameViewportRenderer _mapRenderer = new();
  private readonly BufferedConsoleInput _input = new();
  private readonly GameScreen _screen = new();

  public void Run() {
    Console.Clear();
    Console.CursorVisible = false;
    _consoleRenderer.SetSize(120, 29);

    // InitTestMap();
    
    string[] maze = MazeGenerator.GenerateValidMaze(20, 15, 10, 1);
    MazeGenerator.DrawColoredMaze(maze);
    
    Console.ReadKey();
    
    GameMap map = new FileMapGenerator("PlaygroundMap.txt").BuildFromStrings(maze);
    
    // GameMap map = new FileMapGenerator("PlaygroundMap.txt").Build();
    _gameState.GoToMap(map, _input);

    // Test rendering - when needed to show generated map.
    // MiniMapRenderer mapRenderer = new MiniMapRenderer();
    // mapRenderer.Render(_consoleRenderer.Buffer, _gameState.GetMap());
    // Console.ReadKey();

    var gameLoop = new GameLoop();

    gameLoop
      // Actually, we won't get more than 60 fps, as we use Thread.Sleep and it gives a minimal
      // delay of 15 ms, so we set target fps a bit bigger to get around 64-65 fps.
      .SetTargetFps(70) 
      .SetStepFunction((frameContext) => {
        ProcessInput();
        ProcessFrame(frameContext);
        return _gameRunning;
      })
      .Run();
  }

  private void InitTestMap() {
    IMapGenerator mapGen = new TestMapGenerator();

    var map = mapGen
      .SetSize(40, 13)
      .Build();

    _gameState.GoToMap(map, _input);
  }

  private void ProcessInput() {
    _input.Update();

    var key = _input.GetKey();

    switch (key) {
      case ConsoleKey.Escape:
        OnQuit();
        break;

      case ConsoleKey.D1:
        GameMap map = new FileMapGenerator("PlaygroundMap.txt").Build();
        _gameState.GoToMap(map, _input);
        break;
    }
  }

  private void ProcessFrame(IFrameContext frameContext) {
    _gameState.RemoveMarkedForDestruction();

    var allObjects = _gameState.GetObjects();

    foreach (var obj in allObjects) {
      obj.Update(frameContext.DeltaTime);
    }

    var collidables = _gameState.GetObjectsOfType<ICollidable>();
    var layer = _gameState.GetMap().GetLayer<TileLayer>(GameMap.LayerSolidsId);

    CollisionSystem.CheckCollisionsWithTiles(collidables, layer);
    CollisionSystem.PostCollisionCheck(collidables);

    _screen.Render(_consoleRenderer.Buffer, _gameState.GetMap(), _gameState.Player, frameContext);
    _consoleRenderer.Render();
  }

  private void OnQuit() {
    _gameRunning = false;
  }
}
