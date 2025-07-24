using System.Collections;

using AsciiBlitz.Core;
using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Services;
using AsciiBlitz.Game.Map;
using AsciiBlitz.Game.Objects.Tank;
using AsciiBlitz.Game.Objects.Tank.Controllers;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game;

public interface IGameState {
  /// <summary>
  /// Parameters of maze generation for the first room. If set to null, test map will be loaded from file when
  /// player enters the game room.
  /// </summary>
  MazeGenerationOptions? InitialMazeOptions { get; set; } 
  MazeGenerationOptions? CurrentMazeOptions { get; set; } 
  
  T CreateUnit<T>() where T : UnitObject, new();
  IGameMap GetMap();
  Tank Player { get; }
  IReadonlyStateMachine<GameStage> Stage { get; }
  int Level { get; }
  int Score { get; set; }
  int EnemiesCount { get; }
  int EnemiesDestroyed { get; set; }

  void GoToMap(GameMap map);
  void Reset();
  IReadOnlyList<UnitObject> GetObjects();
  IReadOnlyList<GameObject> GetObjectsOfType<T>();
  void GameOver();
  void GoNextLevel();
  void StartLevel();
}

public class GameState : IGameState {
  public Tank Player { get; private set; }
  public IReadonlyStateMachine<GameStage> Stage => _stage;
  public int Level { get; private set; } = 1;
  public int Score { get; set; } = 0;
  public int EnemiesCount { get; private set; }
  public int EnemiesDestroyed { get; set; }
  public MazeGenerationOptions? InitialMazeOptions { get; set; }
  public MazeGenerationOptions? CurrentMazeOptions { get; set; }
  
  private readonly GameStageStateMachine _stage = new();
  private GameMap _map = new();
  private readonly List<UnitObject> _objects = new();
  private readonly Dictionary<GameObject, int> _objectLayerMap = new();
  private readonly HashSet<UnitObject> _markedForDestruction = new();
  private readonly IGameInput _input = Services.Get<IGameInput>();

  public IGameMap GetMap() {
    return _map;
  }

  public void Update(IFrameContext frameContext) {
    RemoveMarkedForDestruction();
    _stage.Update(frameContext.DeltaTime);
  }

  public void GoToMap(GameMap map) {
    DestroyAll();
    SetMap(map);
    InitPlayer(_input);
    InitEnemies(map.EnemySpawnPoints);

    EnemiesCount = map.EnemySpawnPoints.Count;
  }

  private void InitEnemies(IReadOnlyList<Vec2> spawnPoints) {
    foreach (var point in spawnPoints) {
      var enemy = CreateUnit<Tank>();
      // enemy.Controller = new TankPatrolController(GetMap());
      enemy.Controller = new TankRandomWalkController(GetMap(), Player);
      enemy.Pos = point;
      enemy.OnVisit = (pos) => {
        GetMap().TileVisited(pos, enemy.Dir);
      };
    }
  }

  public void SetMap(GameMap map) {
    _map = map;
  }

  public T CreateUnit<T>() where T : UnitObject, new() {
    return CreateObject<T>(GameMap.LayerObjectsId);
  }

  public T CreateObject<T>(int layerId) where T : UnitObject, new() {
    ObjectLayer layer = _map.GetLayer<ObjectLayer>(layerId);
    T obj = new T();
    obj.GameState = this;

    obj.OnDestroyed += DestroyObject;

    layer.Add(obj);
    AddObject(obj);

    _objectLayerMap[obj] = layerId;

    return obj;
  }

  private void DestroyAll() {
    foreach (var obj in _objects) {
      DestroyObject(obj);
    }
  }

  public void DestroyObject(GameObject obj) {
    if (_objectLayerMap.TryGetValue(obj, out int layerId)) {
      var layer = _map.GetLayer<ObjectLayer>(layerId);

      if (obj is UnitObject unit) {
        layer.Remove(unit);
        _markedForDestruction.Add(unit);
      }

      _objectLayerMap.Remove(obj);
      obj.OnDestroyed -= DestroyObject;
    }
  }

  public IReadOnlyList<UnitObject> GetObjects() {
    return _objects.Where((obj) => !_markedForDestruction.Contains(obj)).ToList();
  }

  public IReadOnlyList<GameObject> GetObjectsOfType<T>() {
    return _objects
      .Where((obj) => obj is T && !_markedForDestruction.Contains(obj))
      .ToList();
  }

  private void AddObject(UnitObject obj) {
    _objects.Add(obj);
  }

  /**
   * Must be called in the very beginning of the game cycle.
   */
  public void RemoveMarkedForDestruction() {
    foreach (var obj in _markedForDestruction) {
      _objects.Remove(obj);
    }

    _markedForDestruction.Clear();
  }

  private void InitPlayer(IGameInput input) {
    Player = CreateUnit<Tank>();
    Player.Controller = new TankConsoleInputController(input);
    Player.IsPlayer = true;
    Player.SetMaxHealth(9);
    Player.ResetHealth();
    Player.Pos = GetMap().PlayerSpawnPoint;
    Player.OnVisit = (pos) => {
      GetMap().TileVisited(pos, Player.Dir);
    };
  }

  public void StartLevel() {
    _stage.Go(GameStage.InGame);
  }

  public void GameOver() {
    _stage.Go(GameStage.GameOver);
  }

  public void Reset() {
    Score = 0;
    Level = 1;
    _stage.Go(GameStage.LevelIntro);
  }

  public void GoNextLevel() {
    Level++;
    EnemiesDestroyed = 0;
    _stage.Go(GameStage.LevelIntro);
  }
}
