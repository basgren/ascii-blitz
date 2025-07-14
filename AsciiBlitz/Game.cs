using AsciiBlitz.Core;
using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Generator;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz;

public class Game {
  private IGameInput _input = new ConsoleInput();

  private bool _gameRunning = true;
  private readonly GameState _gameState = new();
  
  private MapTank Player => _gameState.Player;
  private MapGridRenderer _mapRenderer = new();

  public void Run() {
    Console.Clear();
    Console.CursorVisible = false;
    Console.WriteLine("ASCII Blitz Game");
    InitInput();


    IMapGenerator mapGen = new TestMapGenerator();

    var map = mapGen
      .SetSize(40, 10)
      .Build();

    _gameState.SetMap(map);
    _gameState.Init();
    
    Player.Pos = new Vec2(1, 1);
    
    // Test rendering - when needed to show generated map.
    // GameMapRenderer mapRenderer = new GameMapRenderer();
    // mapRenderer.Render(map);

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
    // Important to make a copy using ToList, as some objects may be destroyed. Should not
    // have performance issues with small number of objects.
    foreach (var obj in _gameState.GetObjects().ToList()) {
      obj.Update(deltaTime);
    }
      
    _input.Update();
      
    _mapRenderer.Render(_gameState.GetMap(), timeFromStart);
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
    var bullet = _gameState.CreateUnit<Projectile>();
    bullet.Speed = VecUtils.DirToVec2(Player.Dir) * 10f;
    bullet.Pos = Player.GetShootPoint();
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

    if (!offs.IsZero && _gameState.GetMap().CanMove(player.Pos, offs)) {
      var newPos = player.Pos + offs;
      var oldGridPos = MapUtils.PosToGrid(player.Pos);
      var newGridPos = MapUtils.PosToGrid(newPos);
      
      player.Pos = newPos;

      if (oldGridPos != newGridPos) {
        GameObject? obj = _gameState.GetMap().GetLayer<TileLayer>(GameMap.LayerGroundId).GetTileAt(player.Pos);
        obj?.Visited();        
      }
    }
  }
}
