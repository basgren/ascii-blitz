using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Game.Tiles;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Map;

public class GameMapFactory {
  private readonly string _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Maps");

  public GameMap CreateFromFile(string filename) {
    string path = Path.Combine(_basePath, filename);
    if (!File.Exists(path))
      throw new FileNotFoundException($"Map file not found: {path}");

    string[] lines = File.ReadAllLines(path)
      .Where(line => !string.IsNullOrWhiteSpace(line))
      .ToArray();

    return CreateFromStrings(lines);
  }
  
  public GameMap CreateFromStrings(string[] lines) {
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
            ground.Add(TileFactory.Create<SoilTile>(gridPos));
            break;
          case MapSymbols.Enemy:
            map.AddEnemySpawnPoint(pos);
            ground.Add(TileFactory.Create<SoilTile>(gridPos));
            break;
          default:
            // Add soil for any unknown symbol
            
            ground.Add(TileFactory.Create<SoilTile>(gridPos));
            break;
        }
      }
    }

    return map; 
  }
}
