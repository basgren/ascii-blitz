using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map;

public class GameMap {
  public int Width => _width;
  public int Height => _height;
  public int LayerCount => _layers.Count;
  public IReadOnlyList<IMapLayer> Layers => _layers.AsReadOnly();
  
  private readonly List<IMapLayer> _layers;
  private int _width;
  private int _height;
  private ObjectLayer? _unitLayer;

  public GameMap() {
    _layers = new List<IMapLayer>();
  }

  public void SetSize(int width, int height) {
    _width = width;
    _height = height;

    foreach (var layer in _layers) {
      layer.Resize(width, height);
    }
  }

  public TileLayer AddLayer() {
    var layer = new TileLayer(_width, _height);
    _layers.Add(layer);
    return layer;
  }

  public ObjectLayer AddUnitLayer() {
    if (_unitLayer != null) {
      throw new InvalidOperationException("Unit layer already exists");
    }
    
    _unitLayer = new ObjectLayer();
    _layers.Add(_unitLayer);
    return _unitLayer;
  }

  public ObjectLayer GetUnitLayer() {
    if (_unitLayer == null) {
      throw new InvalidOperationException("Unit layer does not exist");   
    }

    return _unitLayer;
  }

  public IMapLayer GetLayer(int index) {
    return _layers[index];
  }

  public bool CanMove(Vec2Int playerPos, Vec2Int offs) {
    var layer = GetLayer(1);
    
    var newPos = playerPos + offs;
    var obj = layer.GetAt(newPos.X, newPos.Y);

    return obj == null;
  }
}
