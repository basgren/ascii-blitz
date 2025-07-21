using AsciiBlitz.Core.Objects;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Map.Layers;

public class TileLayer(int id, int order) : AbstractMapLayer<TileObject>(id, order) {
  private TileObject?[,] _tiles;
  private List<TileObject>? _allObjects;
  private int _width;
  private int _height;

  public override void Resize(int width, int height) {
    _tiles = new TileObject?[width, height];
    _width = width;
    _height = height;
  }

  public override List<TileObject> GetObjects() {
    if (_allObjects == null) {
      _allObjects = new List<TileObject>();
      
      for (var x = 0; x < _width; x++) {
        for (var y = 0; y < _height; y++) {
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

  public bool HasTileAt(Vec2 pos) {
    return GetTileAt(pos) != null;
  }
  
  public TileObject? GetTileAt(Vec2 pos) {
    if (pos.X < 0 || pos.X >= _width || pos.Y < 0 || pos.Y >= _height) {
      return null;
    }

    var posInt = MapUtils.PosToGrid(pos);
    return _tiles[posInt.X, posInt.Y];
  }
  
  public TileObject? GetTileAt(int x, int y) {
    if (x < 0 || x >= _width || y < 0 || y >= _height) {
      return null;
    }
    
    return _tiles[x, y];
  }
  
  public bool TryGetTileAt(Vec2Int pos, out TileObject? tile) {
    return TryGetTileAt(pos.X, pos.Y, out tile);
  }
  
  public bool TryGetTileAt(int x, int y, out TileObject? tile) {
    tile = null;

    if (x < 0 || y < 0 || x >= _width || y >= _height) {
      return false;
    }
    
    tile = _tiles[x, y];
    
    return tile != null;
  }
  
  private void ClearCache() {
    _allObjects = null;
  }
}
