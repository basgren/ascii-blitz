using AsciiBlitz.Core.Map.Layers;
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

    TileLayer backLayer = map.GetLayer<TileLayer>(GameMap.LayerGroundId);
    Rect(backLayer, 1, map.Height / 2, map.Width - 1, map.Height - 1, MapObjectType.Grass, true);
    
    TileLayer layer = map.GetLayer<TileLayer>(GameMap.LayerSolidsId);
    Rect(layer, 0, 0, map.Width, map.Height, MapObjectType.Wall);
    Rect(layer, 2, map.Height / 2, map.Width - 2, map.Height / 2 + 1, MapObjectType.Wall);
    
    return map;
  }

  private void Rect(TileLayer layer, int x1, int y1, int x2, int y2, MapObjectType type, bool fill = false) {
    if (fill) {
      for (int x = x1; x < x2; x++) {
        for (int y = y1; y < y2; y++) {
          var obj = _factory.Tile(type, new Vec2Int(x, y));
          layer.Add(obj);
        }
      }
    }
    else {
      for (int x = x1; x < x2; x++) {
        var obj = _factory.Tile(type, new Vec2Int(x, y1));
        layer.Add(obj);
      
        var obj2 = _factory.Tile(type, new Vec2Int(x, y2 - 1));
        layer.Add(obj2);
      }
    
      for (int y = y1 + 1; y < y2; y++) {
        var obj = _factory.Tile(type, new Vec2Int(x1, y));
        layer.Add(obj);
      
        var obj2 = _factory.Tile(type, new Vec2Int(x2 - 1, y));
        layer.Add(obj2);
      }      
    }
  }
}
