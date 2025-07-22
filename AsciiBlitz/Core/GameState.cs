using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Game.Objects.Tank;
using AsciiBlitz.Game.Objects.Tank.Controllers;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core;

public interface IGameState {
  T CreateUnit<T>() where T : UnitObject, new();
  IGameMap GetMap();
}

public class GameState : IGameState {
  public Tank Player { get; private set; }

  private GameMap _map = new();
  private readonly List<UnitObject> _objects = new();
  private readonly Dictionary<GameObject, int> _objectLayerMap = new();
  private HashSet<UnitObject> _markedForDestruction = new();

  // TODO: think about init order - currently we have to add map first, then init Player.

  public IGameMap GetMap() {
    return _map;
  }

  public void GoToMap(GameMap map, BufferedConsoleInput input) {
    DestroyAll();
    SetMap(map);
    InitPlayer(input);
    InitEnemies(map.EnemySpawnPoints);
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
}
