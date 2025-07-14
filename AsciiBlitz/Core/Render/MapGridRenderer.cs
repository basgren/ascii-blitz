using System.Drawing;

using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Render.Sprites;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Render;

public class MapGridRenderer {
  private const int CellWidth = 3;
  private const int CellHeight = 3;

  private readonly Dictionary<MapObjectType, Sprite> _spriteMapping;
  private readonly UnknownSprite _unknownSprite;
  
  private int consoleWidth = ConsoleUtils.Width;
  private int consoleHeight = ConsoleUtils.Height;

  private ScreenBuffer _buffer = new();

  public MapGridRenderer() {
    _spriteMapping = new Dictionary<MapObjectType, Sprite> {
      { MapObjectType.Empty, new EmptySprite() },
      { MapObjectType.Wall, new WallSprite() },
      { MapObjectType.Tank, new TankSprite() },
      { MapObjectType.Grass, new GrassSprite() },
      { MapObjectType.Projectile, new ProjectileSprite() },
    };

    _unknownSprite = new UnknownSprite();
    _buffer.SetSize(Console.WindowWidth, Console.WindowHeight);
  }

  public void Render(GameMap map, double timeFromStartSec) {
    // Get console size to check available space
    consoleWidth = ConsoleUtils.Width;
    consoleHeight = ConsoleUtils.Height;

    // Calculate how many map cells we can fit (each cell is 3x3)
    int maxMapWidth = consoleWidth / 3;
    int maxMapHeight = consoleHeight / 3;

    // Determine the actual render area
    int renderWidth = Math.Min(map.Width, maxMapWidth);
    int renderHeight = Math.Min(map.Height, maxMapHeight);

    // Render the map
    for (int mapY = 0; mapY < renderHeight; mapY++) {
      for (int mapX = 0; mapX < renderWidth; mapX++) {
        GameObject gameObject = GetMapTile(map, mapX, mapY) ?? new EmptyTile(new Vec2Int(mapX, mapY));
        var sprite = GetSpriteForMapObject(gameObject);
        var spriteData = sprite.GetChars(gameObject, timeFromStartSec);
        var colors = sprite.GetColors(gameObject, timeFromStartSec);

        for (int spriteY = 0; spriteY < CellHeight; spriteY++) {
          // Render 3 characters for each map cell
          for (int spriteX = 0; spriteX < CellWidth; spriteX++) {
            var x = mapX * CellWidth + spriteX;
            var y = mapY * CellHeight + spriteY;
            if (x >= consoleWidth || y >= consoleHeight) {
              continue;
            }

            char? c = colors?[spriteX, spriteY];
            int color = c == null ? 7 : GetCharColor(c.Value);
            
            _buffer.Set(x, y, spriteData[spriteX, spriteY], color, 0);
          }
        }
      }
    }
    
    // TODO: implement proper rendering of tiled/object layers. I belive it will be easier to do when we have screen
    //   buffer implemented.
    var layers = map.GetOrderedLayers();

    for (int i = 0; i < layers.Count; i++) {
      var layer = layers[i];

      if (layer is ObjectLayer objLayer) {
        foreach (var obj in objLayer.GetObjects()) {
          RenderObject(obj, timeFromStartSec);
        }
      }
    }
    
    _buffer.RenderChangesOnly();
  }

  private int GetCharColor(char color) {
    return color switch {
      'l' => 0,
      'L' => 8,
      'r' => 1,
      'R' => 9,
      'g' => 2,
      'G' => 10,
      'y' => 3,
      'Y' => 11,
      'b' => 4,
      'B' => 12,
      'p' => 5,
      'P' => 13,
      'c' => 6,
      'C' => 14,
      'w' => 7,
      'W' => 15,
      'x' => 8,
      _ => 0 // Default to no color
    };
  }

  private void RenderObject(MapUnitObject obj, double timeSeconds) {
    var sprite = GetSpriteForMapObject(obj);
    RenderSprite(sprite, obj, obj.Pos, timeSeconds);
  }

  private void RenderSprite(Sprite sprite, GameObject obj, Vec2 pos, double timeSeconds) {
    var spriteData = sprite.GetChars(obj, timeSeconds);
    var colors = sprite.GetColors(obj, timeSeconds);
    
    // Here we round to grid cell, as smooth movement requires more work with collision detections, etc.
    // Vec2Int posInt = MapUtils.PosToGrid(pos);
    
    // int startX = posInt.X * CellWidth;
    // int startY = posInt.Y * CellHeight;
    
    int startX = (int)(pos.X * CellWidth);
    int startY = (int)(pos.Y * CellHeight);
    
    for (int spriteY = 0; spriteY < sprite.Height; spriteY++) {
      // Render 3 characters for each map cell
      for (int spriteX = 0; spriteX < sprite.Width; spriteX++) {
        var x = startX + spriteX;
        var y = startY + spriteY;
        if (x >= consoleWidth || y >= consoleHeight) {
          continue;
        }

        char? c = colors?[spriteX, spriteY];
        int color = c == null ? 7 : GetCharColor(c.Value);
        
        _buffer.Set(x, y, spriteData[spriteX, spriteY], color, 0);
      }
    }
  }

  private TileObject? GetMapTile(GameMap map, int x, int y) {
    // Check layers from highest index to lowest (back to front)
    var layers = map.GetOrderedLayers();

    for (int layerIndex = layers.Count - 1; layerIndex >= 0; layerIndex--) {
      var layer = layers[layerIndex];

      if (layer is TileLayer tileLayer) {
        var tile = tileLayer.GetTileAt(x, y);

        if (tile != null) {
          return tile;
        }        
      }
    }

    return null;
  }

  private Sprite GetSpriteForMapObject(GameObject gameObject) {
    if (_spriteMapping.TryGetValue(gameObject.Type, out var sprite)) {
      return sprite;
    }

    // Return unknown sprite for unmapped objects
    return _unknownSprite;
  }
}
