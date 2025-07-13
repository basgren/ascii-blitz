using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Map.Layers;

public class TileLayer : IMapLayer {
  private MapObject?[,] _data;
  private List<MapObject> _objects = new();

  public TileLayer(int width, int height) {
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
