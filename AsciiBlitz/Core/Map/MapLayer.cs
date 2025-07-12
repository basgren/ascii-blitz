using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Map;

public interface IMapLayer {
  public void Resize(int width, int height);
  public MapObject? GetAt(int x, int y);
  public void SetAt(int x, int y, MapObject? obj);
}

public class MapUnitLayer : IMapLayer {
  public MapTank Player => _tank;
  
  private List<MapUnitObject> _units = new();
  private MapTank _tank = new();

  public MapUnitLayer() {
    AddUnit(_tank); 
  }
  
  public MapObject? GetAt(int x, int y) {
    foreach (var unit in _units) {
      if (unit.Pos.X == x && unit.Pos.Y == y) {
        return unit;
      }
    }
    
    return null;
  }

  public void Resize(int width, int height) {
    // We don't care about resizing
  }
  
  public void SetAt(int x, int y, MapObject? obj) {
  }

  public void AddUnit(MapUnitObject obj) {
    _units.Add(obj);
  }
}

public class MapLayer : IMapLayer {
  private MapObject?[,] _data;
  private List<MapObject> _objects = new();

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
