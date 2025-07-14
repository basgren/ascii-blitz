using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Layers;

public class TileLayer(int id, int order) : AbstractMapLayer<TileObject>(id, order) {
  private TileObject?[,] _tiles;
  private List<TileObject>? _allObjects;

  public override void Resize(int width, int height) {
    _tiles = new TileObject?[width, height];
  }

  public override List<TileObject> GetObjects() {
    if (_allObjects == null) {
      _allObjects = new List<TileObject>();

      int width = _tiles.GetLength(0);
      int height = _tiles.GetLength(1);
      
      for (var x = 0; x < width; x++) {
        for (var y = 0; y < height; y++) {
          var tile = _tiles[x, y];

          if (tile != null) {
            _allObjects.Add(tile);
          }
        }
      }
    }

    return _allObjects;
  }

  public override void Add(TileObject obj) {
    _tiles[obj.Pos.X, obj.Pos.Y] = obj;

    ClearCache();
  }

  public override void Remove(TileObject obj) {
    _tiles[obj.Pos.X, obj.Pos.Y] = null;
    
    ClearCache();
  }

  public bool HasTileAt(Vec2Int pos) {
    return GetTileAt(pos) != null;
  }
  
  public bool HasTileAt(Vec2 pos) {
    return GetTileAt(pos) != null;
  }
  
  public TileObject? GetTileAt(Vec2 pos) {
    var posInt = MapUtils.PosToGrid(pos);
    return _tiles[posInt.X, posInt.Y];
  }
  
  public TileObject? GetTileAt(int x, int y) {
    return _tiles[x, y];
  }
  
  public TileObject? GetTileAt(Vec2Int pos) {
    return _tiles[pos.X, pos.Y];
  }

  private void ClearCache() {
    _allObjects = null;
  }
}
