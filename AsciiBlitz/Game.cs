using AsciiBlitz.Core.Core;
using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Generator;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core;

public class Game {
  private GameMap _map;
  private IGameInput _input = new ConsoleInput();

  private bool _gameRunning = true;
  private readonly GameState _gameState = new();

  public void Run() {
    Console.Clear();
    Console.CursorVisible = false;
    Console.WriteLine("ASCII Blitz Game");
    InitInput();


    IMapGenerator mapGen = new TestMapGenerator();

    _map = mapGen
      .SetSize(20, 10)
      .Build();

    ObjectLayer tankLayer = _map.GetLayer<ObjectLayer>(GameMap.LayerObjectsId);
    _gameState.Player.Pos = new Vec2(1, 1);
    tankLayer.Add(_gameState.Player);
    
    // Test rendering - when needed to show generated map.
    // GameMapRenderer mapRenderer = new GameMapRenderer();
    // mapRenderer.Render(map);

    MapGridRenderer mapRenderer = new();

    // Main game loop - 60 FPS
    const int targetFps = 30;
    const int frameTimeMs = 1000 / targetFps;

    var startTime = DateTime.Now;
    var lastFrameTime = DateTime.Now;

    while (_gameRunning) {
      var currentTime = DateTime.Now;
      var deltaTime = (currentTime - lastFrameTime).TotalMilliseconds;
      var totalTimeSeconds = (currentTime - startTime).TotalSeconds;

      _input.Update();

      mapRenderer.Render(_map, totalTimeSeconds);

      // Frame rate limiting
      if (deltaTime < frameTimeMs) {
        Thread.Sleep((int)(frameTimeMs - deltaTime));
      }

      lastFrameTime = DateTime.Now;
    }
  }

  private void InitInput() {
    _input.MoveLeft += OnMoveLeft;
    _input.MoveRight += OnMoveRight;
    _input.MoveUp += OnMoveUp;
    _input.MoveDown += OnMoveDown;
    _input.Fire += OnFire;
    _input.Quit += OnQuit;
  }

  private void OnMoveLeft() {
    TryMove(Direction.Left);
  }

  private void OnMoveRight() {
    TryMove(Direction.Right);
  }

  private void OnMoveUp() {
    TryMove(Direction.Up);
  }

  private void OnMoveDown() {
    TryMove(Direction.Down);
  }
  
  private void OnFire() {
    // do nothing for now
  }

  private void OnQuit() {
    _gameRunning = false;
  }

  private void TryMove(Direction dir) {
    Vec2 offs = Vec2.Zero;

    switch (dir) {
      case Direction.Up:
        offs = Vec2.Up;
        break;

      case Direction.Down:
        offs = Vec2.Down;
        break;

      case Direction.Left:
        offs = Vec2.Left;
        break;

      case Direction.Right:
        offs = Vec2.Right;
        break;
    }

    MapTank player = _gameState.Player;
    player.Dir = dir;

    if (!offs.IsZero && _map.CanMove(player.Pos, offs)) {
      var newPos = player.Pos + offs;
      var oldGridPos = MapUtils.PosToGrid(player.Pos);
      var newGridPos = MapUtils.PosToGrid(newPos);
      
      player.Pos = newPos;

      if (oldGridPos != newGridPos) {
        MapObject? obj = _map.GetLayer<TileLayer>(GameMap.LayerGroundId).GetTileAt(player.Pos);
        obj?.Visited();        
      }
    }
  }
}
