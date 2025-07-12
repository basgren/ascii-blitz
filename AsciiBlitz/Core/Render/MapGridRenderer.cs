using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Core.Render;

public class MapGridRenderer {
  private const int CellWidth = 3;
  private const int CellHeight = 3;
    
  private readonly Dictionary<MapObjectType, Sprite> _spriteMapping;
  private readonly UnknownSprite _unknownSprite;

  public MapGridRenderer() {
    _spriteMapping = new Dictionary<MapObjectType, Sprite> {
      { MapObjectType.Empty, new EmptySprite() },
      { MapObjectType.Wall, new WallSprite() },
      { MapObjectType.Tank, new TankSprite() },
    };

    _unknownSprite = new UnknownSprite();
  }

  public void Render(GameMap map) {
    // Get console size to check available space
    int consoleWidth = Console.WindowWidth;
    int consoleHeight = Console.WindowHeight;

    // Calculate how many map cells we can fit (each cell is 3x3)
    int maxMapWidth = consoleWidth / 3;
    int maxMapHeight = consoleHeight / 3;

    // Determine the actual render area
    int renderWidth = Math.Min(map.Width, maxMapWidth);
    int renderHeight = Math.Min(map.Height, maxMapHeight);

    // Render the map
    for (int mapY = 0; mapY < renderHeight; mapY++) {
      for (int mapX = 0; mapX < renderWidth; mapX++) {
        MapObject mapObject = GetMapObject(map, mapX, mapY) ?? MapEmpty.Instance;
        var sprite = GetSpriteForMapObject(mapObject);
        var spriteData = sprite.GetSprite(mapObject);
        
        for (int spriteY = 0; spriteY < CellHeight; spriteY++) {
          // Render 3 characters for each map cell
          for (int spriteX = 0; spriteX < CellWidth; spriteX++) {
            var x = mapX * CellWidth + spriteX;
            var y = mapY * CellHeight + spriteY;
            if (x >= consoleWidth || y >= consoleHeight) {
              continue;
            }
            
            Console.SetCursorPosition(x, y);
            Console.Write(spriteData[spriteX, spriteY]);
          }
        }
      }
    }
  }

  private MapObject? GetMapObject(GameMap map, int x, int y) {
    // Check layers from highest index to lowest (back to front)
    for (int layerIndex = map.LayerCount - 1; layerIndex >= 0; layerIndex--) {
      var layer = map.GetLayer(layerIndex);
      var mapObject = layer.GetAt(x, y);

      if (mapObject != null) {
        return mapObject;
      }
    }

    return null;
  }
  
  private Sprite GetSpriteForPosition(GameMap map, int x, int y) {
    // Check layers from highest index to lowest (back to front)
    for (int layerIndex = map.LayerCount - 1; layerIndex >= 0; layerIndex--) {
      var layer = map.GetLayer(layerIndex);
      var mapObject = layer.GetAt(x, y);

      if (mapObject != null) {
        // Return sprite for the first non-null object found
        return GetSpriteForMapObject(mapObject);
      }
    }

    // If no object found in any layer, render as empty sprite
    return _spriteMapping[MapObjectType.Empty];
  }

  private Sprite GetSpriteForMapObject(MapObject mapObject) {
    if (_spriteMapping.TryGetValue(mapObject.Type, out var sprite)) {
      return sprite;
    }

    // Return unknown sprite for unmapped objects
    return _unknownSprite;
  }

  public void RegisterSprite(MapObjectType objectType, Sprite sprite) {
    _spriteMapping[objectType] = sprite;
  }
}
