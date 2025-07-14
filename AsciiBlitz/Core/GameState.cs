using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core;

public class GameState {
  public MapTank Player { get; private set; }

  private GameMap _map = new GameMap();
  private readonly List<MapUnitObject> _objects = new();
  private readonly Dictionary<IDestroyable, int> _objectLayerMap = new();

  // TODO: think about init order - currently we have to add map first, then init Player.

  public void Init() {
    Player = CreateObject<MapTank>(GameMap.LayerObjectsId);
  }

  public GameMap GetMap() {
    return _map;
  }

  public void SetMap(GameMap map) {
    _map = map;
  }

  public T CreateUnit<T>() where T : MapUnitObject, new() {
    return CreateObject<T>(GameMap.LayerObjectsId);
  }

  public T CreateObject<T>(int layerId) where T : MapUnitObject, new() {
    ObjectLayer layer = _map.GetLayer<ObjectLayer>(layerId);
    T obj = new T();

    obj.OnDestroyed += DestroyObject;

    layer.Add(obj);
    AddObject(obj);

    _objectLayerMap[obj] = layerId;

    return obj;
  }

  public void DestroyObject(IDestroyable obj) {
    if (_objectLayerMap.TryGetValue(obj, out int layerId)) {
      var layer = _map.GetLayer<ObjectLayer>(layerId);

      if (obj is MapUnitObject unit) {
        layer.Remove(unit);
        RemoveObject(unit);
      }

      _objectLayerMap.Remove(obj);
      obj.OnDestroyed -= DestroyObject;
    }
  }

  private void AddObject(MapUnitObject obj) {
    _objects.Add(obj);
  }

  private void RemoveObject(MapUnitObject obj) {
    _objects.Remove(obj);
  }

  public IReadOnlyList<MapUnitObject> GetObjects() {
    return _objects;
  }
  
  public IReadOnlyList<T> GetObjectsOfType<T>()
  {
    return _objects.OfType<T>().ToList();
  }
}
