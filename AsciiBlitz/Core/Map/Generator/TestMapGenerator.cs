using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Game.Tiles;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Map.Generator;

public class TestMapGenerator : IMapGenerator {
  Vec2Int _size = new(5, 5);

  public IMapGenerator SetSize(int width, int height) {
    _size = new Vec2Int(width, height);
    return this;
  }

  public GameMap Build() {
    GameMap map = new GameMap();

    map.SetSize(_size.X, _size.Y);

    TileLayer backLayer = map.GetLayer<TileLayer>(GameMap.LayerGroundId);
    Rect<GrassTile>(backLayer, 1, map.Height / 2, map.Width - 1, map.Height - 1, true);

    TileLayer solid = map.GetLayer<TileLayer>(GameMap.LayerSolidsId);
    Rect<WallTile>(solid, 0, 0, map.Width, map.Height);
    Rect<WallTile>(solid, 2, map.Height / 2, map.Width - 2, map.Height / 2 + 1);
    Rect<WeakWallTile>(solid, 2, map.Height / 2, map.Width / 2, map.Height / 2 + 1);

    map.PlayerSpawnPoint = new Vec2(1, 1);
    map.AddEnemySpawnPoint(new Vec2(1, 3));
    
    return map;
  }

  private void Rect<T>(TileLayer layer, int x1, int y1, int x2, int y2, bool fill = false) where T : TileObject, new() {
    if (fill) {
      for (int x = x1; x < x2; x++) {
        for (int y = y1; y < y2; y++) {
          var obj = TileFactory.Create<T>(new Vec2Int(x, y));
          layer.Add(obj);
        }
      }
    }
    else {
      for (int x = x1; x < x2; x++) {
        var obj = TileFactory.Create<T>(new Vec2Int(x, y1));
        layer.Add(obj);

        var obj2 = TileFactory.Create<T>(new Vec2Int(x, y2 - 1));
        layer.Add(obj2);
      }

      for (int y = y1 + 1; y < y2; y++) {
        var obj = TileFactory.Create<T>(new Vec2Int(x1, y));
        layer.Add(obj);

        var obj2 = TileFactory.Create<T>(new Vec2Int(x2 - 1, y));
        layer.Add(obj2);
      }
    }
  }
}
