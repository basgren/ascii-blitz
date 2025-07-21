using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Game.Tiles;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Map;

public interface IGameMap {
  int Width { get; }
  int Height { get; }
  T GetLayer<T>(int id) where T : AbstractMapLayer;
  IReadOnlyList<AbstractMapLayer> GetOrderedLayers();
  bool IsMovable(Vec2 pos);
  Vec2 PlayerSpawnPoint { get; }
  bool IsVisionTransparent(Vec2Int pos);
  void TileVisited(Vec2 pos, Direction dir);
}

public class GameMap : IGameMap {
  public const int LayerGroundId = 1;
  public const int LayerSolidsId = 2;
  public const int LayerObjectsId = 3;

  public int Width => _width;
  public int Height => _height;

  public Vec2 PlayerSpawnPoint { get; set; } = new(1, 1);
  public IReadOnlyList<Vec2> EnemySpawnPoints => _enemySpawnPoints;

  private readonly Dictionary<int, AbstractMapLayer> _layers;
  private List<AbstractMapLayer>? _orderedLayers;
  private int _width;
  private int _height;
  private readonly List<Vec2> _enemySpawnPoints = new();

  public GameMap() {
    _layers = new Dictionary<int, AbstractMapLayer>();

    AddTileLayer(LayerGroundId, 10);
    AddTileLayer(LayerSolidsId, 20);
    AddUnitLayer(LayerObjectsId, 30);
  }

  public void SetSize(int width, int height) {
    _width = width;
    _height = height;

    foreach (var layer in _layers.Values) {
      layer.Resize(width, height);
    }
  }

  public TileLayer AddTileLayer(int id, int order) {
    var layer = new TileLayer(id, order);
    _layers.Add(id, layer);
    ClearCache();
    return layer;
  }

  public ObjectLayer AddUnitLayer(int id, int order) {
    var layer = new ObjectLayer(id, order);
    _layers.Add(id, layer);
    ClearCache();
    return layer;
  }

  public IReadOnlyList<AbstractMapLayer> GetOrderedLayers() {
    if (_orderedLayers == null) {
      _orderedLayers = _layers.Values.OrderBy(x => x.Order).ToList();
    }
    
    return _orderedLayers;
  }

  public bool IsMovable(Vec2 pos) {
    if (pos.X < 0 || pos.X >= _width || pos.Y < 0 || pos.Y >= _height) {
      return false;
    }
    
    TileLayer layer = GetLayer<TileLayer>(LayerSolidsId);
    return !layer.HasTileAt(pos);
  }
  
  public bool IsVisionTransparent(Vec2Int pos) {
    TileLayer layer = GetLayer<TileLayer>(LayerSolidsId);
    
    if (layer.TryGetTileAt(pos, out var tile)) {
      return tile is not WallTile && tile is not WeakWallTile;
    }

    return true;
  }

  public T GetLayer<T>(int id) where T : AbstractMapLayer {
    if (!_layers.TryGetValue(id, out var layer)) {
      throw new KeyNotFoundException($"Layer with ID {id} was not found.");      
    }

    if (layer is not T typedLayer) {
      throw new InvalidCastException($"Layer with ID {id} is of type {layer.GetType().Name}, not {typeof(T).Name}.");      
    }

    return typedLayer;
  }
  
  public void TileVisited(Vec2 pos, Direction dir) {
    TileObject? tile = GetLayer<TileLayer>(LayerGroundId).GetTileAt(pos);

    if (tile is GroundTile groundTile) {
      groundTile.Visited(dir);
    }
  }

  private void ClearCache() {
    _orderedLayers = null;
  }

  public void AddEnemySpawnPoint(Vec2 pos) {
    _enemySpawnPoints.Add(pos);    
  }
}
