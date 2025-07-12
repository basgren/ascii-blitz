using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Generator;

public class TestMapGenerator : IMapGenerator {
  Vec2Int _size = new Vec2Int(5, 5);

  private readonly MapObjectFactory _factory = new();
  
  public IMapGenerator SetSize(int width, int height) {
    _size = new Vec2Int(width, height);
    return this;
  }

  public GameMap Build() {
    GameMap map = new GameMap();

    map.SetSize(_size.X, _size.Y);
    
    MapLayer layer = map.AddLayer();

    Rect(layer, 0, 0, map.Width, map.Height, MapObjectType.Wall);
    
    MapUnitLayer tankLayer = map.AddUnitLayer();
    tankLayer.Player.Pos = new Vec2Int(1, 1);
    
    return map;
  }

  private void Rect(MapLayer layer, int x1, int y1, int x2, int y2, MapObjectType type) {
    for (int x = x1; x < x2; x++) {
      var obj = _factory.Create(type);
      layer.SetAt(x, y1, obj);
      
      var obj2 = _factory.Create(type);
      layer.SetAt(x, y2 - 1, obj2);
    }
    
    for (int y = y1 + 1; y < y2; y++) {
      var obj = _factory.Create(type);
      layer.SetAt(x1, y, obj);
      
      var obj2 = _factory.Create(type);
      layer.SetAt(x2 - 1, y, obj2);
    }
  }
}
