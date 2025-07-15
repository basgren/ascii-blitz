using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Types;
using AsciiBlitz.Game.Objects;

namespace AsciiBlitz.Core;

public class GameState {
  public Tank Player { get; private set; }

  private GameMap _map = new();
  private readonly List<UnitObject> _objects = new();
  private readonly Dictionary<GameObject, int> _objectLayerMap = new();

  // TODO: think about init order - currently we have to add map first, then init Player.

  public void Init() {
    Player = CreateUnit<Tank>();
  }

  public GameMap GetMap() {
    return _map;
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

    obj.OnDestroyed += DestroyObject;

    layer.Add(obj);
    AddObject(obj);

    _objectLayerMap[obj] = layerId;

    return obj;
  }

  public void DestroyObject(GameObject obj) {
    if (_objectLayerMap.TryGetValue(obj, out int layerId)) {
      var layer = _map.GetLayer<ObjectLayer>(layerId);

      if (obj is UnitObject unit) {
        layer.Remove(unit);
        RemoveObject(unit);
      }

      _objectLayerMap.Remove(obj);
      obj.OnDestroyed -= DestroyObject;
    }
  }

  private void AddObject(UnitObject obj) {
    _objects.Add(obj);
  }

  private void RemoveObject(UnitObject obj) {
    _objects.Remove(obj);
  }

  public IReadOnlyList<UnitObject> GetObjects() {
    return _objects;
  }
  
  public IReadOnlyList<T> GetObjectsOfType<T>()
  {
    return _objects.OfType<T>().ToList();
  }
}
