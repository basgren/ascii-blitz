using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Map.Layers;

public class ObjectLayer : IMapLayer {
  public MapTank Player => _tank;
  
  private List<MapUnitObject> _units = new();
  private MapTank _tank = new();

  public ObjectLayer() {
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
