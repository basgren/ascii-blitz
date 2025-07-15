using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Core.Render;

public class MapGridRenderer {
  private const int CellWidth = 3;
  private const int CellHeight = 3;

  // private readonly Dictionary<MapObjectType, Sprite> _spriteMapping;
  private readonly UnknownSprite _unknownSprite;
  
  private int _consoleWidth = ConsoleUtils.Width;
  private int _consoleHeight = ConsoleUtils.Height;

  private ScreenBuffer _buffer = new();

  public MapGridRenderer() {
    _unknownSprite = new UnknownSprite();
    _buffer.SetSize(Console.WindowWidth, Console.WindowHeight);
  }

  public void Render(GameMap map, double timeFromStartSec) {
    _buffer.Clear();

    // Get console size to check available space
    _consoleWidth = ConsoleUtils.Width;
    _consoleHeight = ConsoleUtils.Height;

    // Calculate how many map cells we can fit (each cell is 3x3)
    int maxMapWidth = _consoleWidth / 3;
    int maxMapHeight = _consoleHeight / 3;

    // Determine the actual render area
    int renderWidth = Math.Min(map.Width, maxMapWidth);
    int renderHeight = Math.Min(map.Height, maxMapHeight);

    // Render the map
    for (int mapY = 0; mapY < renderHeight; mapY++) {
      for (int mapX = 0; mapX < renderWidth; mapX++) {
        GameObject? gameObject = GetMapTile(map, mapX, mapY);

        RenderGameObjectSprite(gameObject, mapX * CellWidth, mapY * CellHeight, timeFromStartSec);
      }
    }
    
    // TODO: implement proper rendering of tiled/object layers. I belive it will be easier to do when we have screen
    //   buffer implemented.
    var layers = map.GetOrderedLayers();

    for (int i = 0; i < layers.Count; i++) {
      var layer = layers[i];

      if (layer is ObjectLayer objLayer) {
        foreach (var obj in objLayer.GetObjects()) {
          int x = (int)(obj.Pos.X * CellWidth);
          int y = (int)(obj.Pos.Y * CellHeight);

          RenderGameObjectSprite(obj, x, y, timeFromStartSec);
        }
      }
    }
    
    _buffer.RenderChangesOnly();
  }

  private void RenderGameObjectSprite(GameObject? gameObject, int screenX, int screenY, double gameTimeSec) {
    var sprite = gameObject?.Sprite;

    if (sprite == null) {
      return; 
    }
    
    ScreenCell[,] cells = sprite.UpdateAll(gameObject, gameTimeSec);

    for (int spriteY = 0; spriteY < sprite.Height; spriteY++) {
      // Render 3 characters for each map cell
      for (int spriteX = 0; spriteX < sprite.Width; spriteX++) {
        var x = screenX + spriteX;
        var y = screenY + spriteY;
            
        _buffer.Set(x, y, cells[spriteX, spriteY]);
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
}
