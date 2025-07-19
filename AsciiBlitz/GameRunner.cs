using AsciiBlitz.Core;
using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Generator;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Game;
using AsciiBlitz.Game.Objects.Tank;

namespace AsciiBlitz;

public class GameRunner {
  private bool _gameRunning = true;
  private readonly GameState _gameState = new();
  
  private Tank Player => _gameState.Player;

  private readonly BufferedConsoleRenderer _consoleRenderer = new();
  private readonly GameViewportRenderer _mapRenderer = new();
  private readonly BufferedConsoleInput _input = new();
  private readonly GameScreen _screen = new();

  public void Run() {
    Console.Clear();
    Console.CursorVisible = false;
    _consoleRenderer.SetSize(120, 29);

    InitTestMap();
    
    // Test rendering - when needed to show generated map.
    // MiniMapRenderer mapRenderer = new MiniMapRenderer();
    // mapRenderer.Render(_consoleRenderer.Buffer, _gameState.GetMap());
    // Console.ReadKey();
    
    // Main game loop - 50 FPS
    const int targetFps = 50;
    const int frameTimeMs = 1000 / targetFps;

    // Move to game state?
    var gameStartTime = DateTime.Now;
    var deltaTimeSec = 0.0f;

    while (_gameRunning) {
      _gameState.RemoveMarkedForDestruction();
      
      var frameStartTime = DateTime.Now;
      var timeFromGameStart = (float)(frameStartTime - gameStartTime).TotalSeconds;

      ProcessInput();
      ProcessFrame(timeFromGameStart, deltaTimeSec);
      
      var frameProcessingTime = (float)(frameStartTime - DateTime.Now).TotalMilliseconds;
      
      // Frame rate limiting
      if (frameProcessingTime < frameTimeMs) {
        Thread.Sleep((int)(frameTimeMs - frameProcessingTime));
      }

      var frameEndTime = DateTime.Now;
      deltaTimeSec = (float)(frameEndTime - frameStartTime).TotalSeconds;
    }
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

  private void ProcessFrame(float timeFromStart, float deltaTime) {
    var allObjects = _gameState.GetObjects();

    foreach (var obj in allObjects) {
      obj.Update(deltaTime);
    }

    var collidables = _gameState.GetObjectsOfType<ICollidable>();
    var layer = _gameState.GetMap().GetLayer<TileLayer>(GameMap.LayerSolidsId);
    
    CollisionSystem.CheckCollisionsWithTiles(collidables, layer);
    CollisionSystem.PostCollisionCheck(collidables);
    
    // Destroy dead objects
    foreach (var obj in allObjects) {
      if (obj is IDamageable { Damageable.IsDead: true }) {
        obj.Destroy();
      }
    }

    _screen.Render(_consoleRenderer.Buffer, _gameState.GetMap(), _gameState.Player, timeFromStart);
    _consoleRenderer.Render();
  }

  private void OnQuit() {
    _gameRunning = false;
  }
}
