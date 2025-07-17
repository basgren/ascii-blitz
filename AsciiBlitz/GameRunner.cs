using AsciiBlitz.Core;
using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Generator;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Debug;
using AsciiBlitz.Game;
using AsciiBlitz.Game.Objects.Tank;
using AsciiBlitz.Types;

namespace AsciiBlitz;

public class GameRunner {
  private bool _gameRunning = true;
  private readonly GameState _gameState = new();
  
  private Tank Player => _gameState.Player;

  private readonly BufferedConsoleRenderer _consoleRenderer = new();
  private readonly MapGridRenderer _mapRenderer = new();
  private readonly BufferedConsoleInput _input = new();
  private readonly GameScreen _screen = new();

  public void Run() {
    Console.Clear();
    Console.CursorVisible = false;
    _consoleRenderer.SetSize(120, 29);

    IMapGenerator mapGen = new TestMapGenerator();

    var map = mapGen
      .SetSize(40, 13)
      .Build();

    _gameState.SetMap(map);
    _gameState.Init(_input);
    
    Player.Pos = new Vec2(1, 1);
    
    var enemy = _gameState.CreateUnit<Tank>();
    enemy.Controller = new TankPatrolController(_gameState.GetMap());
    enemy.Pos = new Vec2(1, 3);
    
    // Test rendering - when needed to show generated map.
    // GameMapRenderer mapRenderer = new GameMapRenderer();
    // mapRenderer.Render(map);
    // Console.ReadKey();
    
    // Main game loop - 30 FPS
    const int targetFps = 30;
    const int frameTimeMs = 1000 / targetFps;

    // Move to game state?
    var gameStartTime = DateTime.Now;
    var deltaTimeSec = 0.0f;

    while (_gameRunning) {
      var frameStartTime = DateTime.Now;
      var timeFromGameStart = (float)(frameStartTime - gameStartTime).TotalSeconds;
      
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

  private void ProcessFrame(float timeFromStart, float deltaTime) {
    _input.Update();
    
    var key = _input.GetKey();

    if (key == ConsoleKey.Escape) {
      OnQuit();
    }
    
    // Important to make a copy using ToList, as some objects may be destroyed. Should not
    // have performance issues with a small number of objects.
    var allObjects = _gameState.GetObjects().ToList();

    foreach (var obj in allObjects) {
      obj.Update(deltaTime);
    }
 
    // Make a copy of list by using `ToList()` as some objects may be destroyed during check, but
    // still we want to check all collisions.  
    var collidables = _gameState.GetObjectsOfType<ICollidable>().ToList();
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
