using AsciiBlitz.Core;
using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Loop;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Game;
using AsciiBlitz.Game.Map;
using AsciiBlitz.Game.Objects.Tank;

namespace AsciiBlitz;

public class GameRunner {
  private bool _gameRunning = true;
  private readonly GameState _gameState = new();

  private readonly BufferedConsoleRenderer _consoleRenderer = new();
  private readonly BufferedConsoleInput _input = new();
  private readonly GameScreen _screen = new();
  private readonly GameMapFactory _mapFactory = new();
  private MazeGenerationOptions _initialMazeOptions;
  private MazeGenerationOptions _mazeOptions;

  public void Run() {
    Console.Clear();
    Console.CursorVisible = false;
    _consoleRenderer.SetSize(120, 30);

    InitMap(false);

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

  private void InitMap(bool testMap) {
    GameMap map;

    if (testMap) {
      map = _mapFactory.CreateFromFile("PlaygroundMap.txt");
    } else {
      var mapGen = new InteractiveMapGenerator(29, 23, 7888);
      _initialMazeOptions = mapGen.ReadOptions();
      _mazeOptions = _initialMazeOptions;
    
      string [] maze = MazeGenerator.GenerateValidMaze(_mazeOptions);
      map = _mapFactory.CreateFromStrings(maze);
    }
    
    _gameState.GoToMap(map, _input);
    _consoleRenderer.Clear();
  }

  private void ProcessInput() {
    _input.Update();

    var key = _input.GetKey();

    switch (key) {
      case ConsoleKey.Escape:
        OnQuit();
        break;

      case ConsoleKey.D1:
        GameMap map = _mapFactory.CreateFromFile("PlaygroundMap.txt");
        _gameState.GoToMap(map, _input);
        break;
      
      // Debug keys - to quickly destroy enemies or player
      // case ConsoleKey.D9:
      //   _gameState.Player.Damageable.ApplyDamage(1);
      //   _input.Consume();
      //   break;
      //
      // case ConsoleKey.D0:
      //   foreach (var obj in _gameState.GetObjectsOfType<Tank>()) {
      //     if (obj is Tank tank && obj != _gameState.Player) {
      //       tank.Damageable.ApplyDamage(1);
      //       break;
      //     }
      //   }
      //
      //   _input.Consume();
      //   break;
    }
  }

  private void ProcessFrame(IFrameContext frameContext) {
    _gameState.Update(frameContext);
    var stage = _gameState.Stage;

    switch (stage.State) {
      case GameStage.LevelIntro:
        if (_input.GetKey() != null) {
          _gameState.StartLevel();
        }
        break;
      
      case GameStage.InGame:
        ProcessInGameFrame(frameContext);
        break;
      
      case GameStage.GameOver:
        if (_input.GetKey() != null) {
          InitMap(false);
          _gameState.Reset();
        }
        break;

      default:
        throw new ArgumentOutOfRangeException();
    }

    _screen.Render(_consoleRenderer.Buffer, _gameState, frameContext);
    _consoleRenderer.Render();
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
        obj.Destroy();
        
        if (obj == _gameState.Player) {
          _gameState.GameOver();
        } else if (obj is Tank) {
          _gameState.EnemiesDestroyed++;
          _gameState.Score += 10;
          
          if (_gameState.EnemiesDestroyed >= _gameState.EnemiesCount) {
            GoNextLevel();
          }
        }
      }
    }
  }

  private void GoNextLevel() {
    _gameState.GoNextLevel();
    
    _mazeOptions.Width = _initialMazeOptions.Width + (_gameState.Level - 1) * 2;
    _mazeOptions.Height = _initialMazeOptions.Width + _gameState.Level - 1;
    _mazeOptions.Seed = _initialMazeOptions.Seed + _gameState.Level;
    
    string [] maze = MazeGenerator.GenerateValidMaze(_mazeOptions);
    var map = _mapFactory.CreateFromStrings(maze);
    _gameState.GoToMap(map, _input);
  }

  private void OnQuit() {
    _gameRunning = false;
  }
}
