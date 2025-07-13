using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Map.Layers;

public class TileLayer(int id, int order) : AbstractMapLayer<MapTile>(id, order) {
  private MapTile?[,] _tiles;
  private List<MapTile>? _allObjects;

  public override void Resize(int width, int height) {
    _tiles = new MapTile?[width, height];
  }

  public override List<MapTile> GetObjects() {
    if (_allObjects == null) {
      _allObjects = new List<MapTile>();

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

  public override void Add(MapTile obj) {
    _tiles[obj.Pos.X, obj.Pos.Y] = obj;

    ClearCache();
  }

  public override void Remove(MapTile obj) {
    _tiles[obj.Pos.X, obj.Pos.Y] = null;
    
    ClearCache();
  }

  public bool HasTileAt(Vec2Int pos) {
    return GetTileAt(pos) != null;
  }
  
  public bool HasTileAt(Vec2 pos) {
    return GetTileAt(pos) != null;
  }
  
  public MapTile? GetTileAt(Vec2 pos) {
    var posInt = MapUtils.PosToGrid(pos);
    return _tiles[posInt.X, posInt.Y];
  }
  
  public MapTile? GetTileAt(int x, int y) {
    return _tiles[x, y];
  }
  
  public MapTile? GetTileAt(Vec2Int pos) {
    return _tiles[pos.X, pos.Y];
  }

  private void ClearCache() {
    _allObjects = null;
  }
}
