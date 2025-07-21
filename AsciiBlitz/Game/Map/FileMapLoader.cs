using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Game.Tiles;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Map;

public class FileMapGenerator : IMapGenerator {
  private string _filename = string.Empty;
  private readonly string _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Maps");
  // private readonly string _basePath = "Assets/Maps";

  public FileMapGenerator(string filename) {
    _filename = filename;
  }

  public IMapGenerator SetSize(int width, int height) {
    // Размер задается автоматически по содержимому файла
    return this;
  }

  public GameMap Build() {
    string path = Path.Combine(_basePath, _filename);
    if (!File.Exists(path))
      throw new FileNotFoundException($"Map file not found: {path}");

    string[] lines = File.ReadAllLines(path)
      .Where(line => !string.IsNullOrWhiteSpace(line))
      .ToArray();

    return BuildFromStrings(lines);
  }
  
  public GameMap BuildFromStrings(string[] lines) {
    int height = lines.Length;
    int width = lines.Max(line => line.Length);

    GameMap map = new GameMap();
    map.SetSize(width, height);

    TileLayer solids = map.GetLayer<TileLayer>(GameMap.LayerSolidsId);
    TileLayer ground = map.GetLayer<TileLayer>(GameMap.LayerGroundId);

    for (int y = 0; y < height; y++) {
      string line = lines[y];
      for (int x = 0; x < line.Length; x++) {
        char c = line[x];
        var pos = new Vec2(x, y);
        var gridPos = MapUtils.PosToGrid(pos);

        switch (c) {
          case MapSymbols.Wall:
            solids.Add(TileFactory.Create<WallTile>(gridPos));
            break;
          case MapSymbols.Destructible:
            solids.Add(TileFactory.Create<WeakWallTile>(gridPos));
            break;
          case MapSymbols.River:
            solids.Add(TileFactory.Create<RiverTile>(gridPos));
            break;
          case MapSymbols.Grass:
            ground.Add(TileFactory.Create<GrassTile>(gridPos));
            break;
          case MapSymbols.Wheat:
            ground.Add(TileFactory.Create<WheatTile>(gridPos));
            break;
          case MapSymbols.Bridge:
            ground.Add(TileFactory.Create<BridgeTile>(gridPos));
            break;
          case MapSymbols.Player:
            map.PlayerSpawnPoint = pos;
            break;
          case MapSymbols.Enemy:
            map.AddEnemySpawnPoint(pos);
            break;
          default:
            // ignore unknown symbols
            break;
        }
      }
    }

    return map; 
  }
}
