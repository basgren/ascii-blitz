using AsciiBlitz.Core;
using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Services;
using AsciiBlitz.Game.Map;
using AsciiBlitz.Game.Objects.Tank;

namespace AsciiBlitz.Game.Rooms.Game;

public class GameRoom : AbstractRoom {
  private const float LevelCompleteDelay = 2f;
  
  private readonly IGameState _gameState = Services.Get<IGameState>();
  private readonly IConsoleRenderer _renderer = Services.Get<IConsoleRenderer>();
  private readonly IRoomService _roomService = Services.Get<IRoomService>();
  private readonly IGameInput _input = Services.Get<IGameInput>();
  private readonly GameScreen _screen = new();

  private readonly GameMapFactory _mapFactory = new();

  public override void OnRoomEnter() {
    // _mazeOptions = _gameState.InitialMazeOptions ?? new MazeGenerationOptions(41, 41, 7888);
    InitMap();
  }

  public override bool ProcessFrame(IFrameContext frameContext) {
    ProcessInput();

    var stage = _gameState.Stage;

    switch (stage.State) {
      case GameStage.LevelIntro:
        if (_input.GetKey() != null) {
          _gameState.StartLevel();
          _input.Consume();
        }
        break;

      case GameStage.InGame:
      case GameStage.LevelComplete:
        ProcessInGameFrame(frameContext);
        break;

      case GameStage.GameOver:
        if (_input.GetKey() != null) {
          InitMap();
          _gameState.Reset();
          _input.Consume();
        }
        break;

      default:
        throw new ArgumentOutOfRangeException();
    }

    _screen.Render(_renderer.Buffer, _gameState, frameContext);

    return true;
  }

  private void ProcessInGameFrame(IFrameContext frameContext) {
    var allObjects = _gameState.GetObjects();

    foreach (var obj in allObjects) {
      obj.Update(frameContext.DeltaTime);
    }

    var collidables = _gameState.GetObjectsOfType<ICollidable>();
    var layer = _gameState.GetMap().GetLayer<TileLayer>(GameMap.LayerSolidsId);

    CollisionSystem.CheckCollisionsWithTiles(collidables, layer);
    CollisionSystem.PostCollisionCheck(collidables);

    foreach (var obj in allObjects) {
      if (obj is IDamageable { Damageable.IsDead: true }) {
        obj.OnBeforeDestroy();
        obj.Destroy();

        if (obj == _gameState.Player) {
          _gameState.GameOver();
        } else if (obj is Tank) {
          _gameState.EnemiesDestroyed++;
          _gameState.Score += 10;

          if (_gameState.EnemiesDestroyed >= _gameState.EnemiesCount) {
            _gameState.LevelComplete(LevelCompleteDelay, GoNextLevel);
          }
        }
      }
    }
  }

  private void GoNextLevel() {
    _gameState.GoNextLevel();
    var options = _gameState.InitialMazeOptions;

    if (options != null) {
      int levelIndex = _gameState.Level - 1;

      _gameState.CurrentMazeOptions = new MazeGenerationOptions(
        options.Width + levelIndex * 2,
        options.Height + levelIndex * 2,
        options.Seed + levelIndex
      );
      
      MazeGenerator.ClampGenerationOptions(_gameState.CurrentMazeOptions);

      string[] maze = MazeGenerator.GenerateValidMaze(_gameState.CurrentMazeOptions);
      var map = _mapFactory.CreateFromStrings(maze);
      _gameState.GoToMap(map);
    }
  }

  private void ProcessInput() {
    var key = _input.GetKey();

    bool processed = true;

    switch (key) {
      case ConsoleKey.Escape:
        _roomService.GoToRoom<MenuRoom>();
        break;

      // Debug keys - to quickly destroy enemies or player - just to test process of dying or win/lose conditions
      case ConsoleKey.F3:
        _gameState.Player.Damageable.ApplyDamage(1);
        _input.Consume();
        break;

      case ConsoleKey.F4:
        foreach (var obj in _gameState.GetObjectsOfType<Tank>()) {
          if (obj is Tank tank && obj != _gameState.Player) {
            tank.Damageable.ApplyDamage(1);
            break;
          }
        }

        _input.Consume();
        break;

      default:
        processed = false;
        break;
    }

    if (processed) {
      _input.Consume();
    }
  }

  private void InitMap() {
    GameMap map;

    if (_gameState.InitialMazeOptions == null) {
      map = _mapFactory.CreateFromFile("PlaygroundMap.txt");
    } else {
      _gameState.CurrentMazeOptions ??= new MazeGenerationOptions(
        _gameState.InitialMazeOptions.Width,
        _gameState.InitialMazeOptions.Height,
        _gameState.InitialMazeOptions.Seed
      );

      string[] maze = MazeGenerator.GenerateValidMaze(_gameState.CurrentMazeOptions);
      map = _mapFactory.CreateFromStrings(maze);
    }

    _gameState.GoToMap(map);
    _renderer.Clear();
  }
}
