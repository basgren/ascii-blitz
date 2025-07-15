using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Types;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Map;

public class GameMap {
  public const int LayerGroundId = 1;
  public const int LayerSolidsId = 2;
  public const int LayerObjectsId = 3;

  public int Width => _width;
  public int Height => _height;
  public int LayerCount => _layers.Count;

  private readonly Dictionary<int, AbstractMapLayer> _layers;
  private List<AbstractMapLayer>? _orderedLayers = null;
  private int _width;
  private int _height;
  private ObjectLayer? _unitLayer;

  public GameMap() {
    _layers = new Dictionary<int, AbstractMapLayer>();

    AddTileLayer(LayerGroundId, 10);
    AddTileLayer(LayerSolidsId, 20);
    AddUnitLayer(LayerObjectsId, 30);
  }

  public void SetSize(int width, int height) {
    _width = width;
    _height = height;

    foreach (var layer in _layers.Values) {
      layer.Resize(width, height);
    }
  }

  public TileLayer AddTileLayer(int id, int order) {
    var layer = new TileLayer(id, order);
    _layers.Add(id, layer);
    ClearCache();
    return layer;
  }

  public ObjectLayer AddUnitLayer(int id, int order) {
    var layer = new ObjectLayer(id, order);
    _layers.Add(id, layer);
    ClearCache();
    return layer;
  }

  public IReadOnlyList<AbstractMapLayer> GetOrderedLayers() {
    if (_orderedLayers == null) {
      _orderedLayers = _layers.Values.OrderBy(x => x.Order).ToList();
    }
    
    return _orderedLayers;
  }
  
  public T GetLayer<T>(int id) where T : AbstractMapLayer {
    if (!_layers.TryGetValue(id, out var layer)) {
      throw new KeyNotFoundException($"Layer with ID {id} was not found.");      
    }

    if (layer is not T typedLayer) {
      throw new InvalidCastException($"Layer with ID {id} is of type {layer.GetType().Name}, not {typeof(T).Name}.");      
    }

    return typedLayer;
  }

  // TODO: move collision detection to more appropriate place.
  public bool CanMove(Vec2 playerPos, Vec2 offs) {
    TileLayer layer = GetLayer<TileLayer>(LayerSolidsId);
    Vec2 newPos = playerPos + offs;
    
    return !layer.HasTileAt(newPos);
  }

  private void ClearCache() {
    _orderedLayers = null;
  }
}
