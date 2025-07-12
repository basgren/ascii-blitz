using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Map;

public class MapLayer {
  private MapObject?[,] _data;
  private List<MapObject> _objects = new List<MapObject>();

  public int Width => _data.GetLength(0);
  public int Height => _data.GetLength(1);

  public MapLayer(int width, int height) {
    _data = new MapObject?[width, height];
  }

  public void Resize(int width, int height) {
    _data = new MapObject?[width, height];
  }

  public MapObject? GetAt(int x, int y) {
    return _data[x, y];
  }

  public void SetAt(int x, int y, MapObject? obj) {
    var existingObj = _data[x, y];
    
    if (existingObj != null) {
      _objects.Remove(existingObj);        
    }
    
    if (obj != null) {
      _objects.Add(obj);
    }
    
    _data[x, y] = obj;
  }
}

public class GameMap {
  private readonly List<MapLayer> _layers;
  private int _width;
  private int _height;

  public int Width => _width;
  public int Height => _height;
  public int LayerCount => _layers.Count;
  public IReadOnlyList<MapLayer> Layers => _layers.AsReadOnly();

  public GameMap() {
    _layers = new List<MapLayer>();
  }

  public void SetSize(int width, int height) {
    _width = width;
    _height = height;

    foreach (var layer in _layers) {
      layer.Resize(width, height);
    }
  }

  public MapLayer AddLayer() {
    var layer = new MapLayer(_width, _height);
    _layers.Add(layer);
    return layer;
  }

  public MapLayer GetLayer(int index) {
    return _layers[index];
  }
}
