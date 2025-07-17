using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Render.Sprites;
using AsciiBlitz.Debug;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Render;

public class MapGridRenderer {
  private const float ScaleX = 3f; // 1 cell on a map will be 3 chars wide on the screen
  private const float ScaleY = 3f; // 1 cell on a map will be 3 chars high on the screen

  // private readonly Dictionary<MapObjectType, Sprite> _spriteMapping;
  private readonly UnknownSprite _unknownSprite = new();

  private Vec2 _cameraCenterWorld = Vec2.Zero;

  public MapGridRenderer SetCameraCenterWorldCoords(Vec2 cameraCenter) {
    _cameraCenterWorld = cameraCenter;
    return this;
  }
  
  public void Render(ScreenBuffer target, IGameMap map, double timeFromStartSec) {
    // Size of visible window on map (not on screen) - only tiles/objects that are
    // visible inside this window will be rendered.
    Vec2 mapWindowSize = new Vec2(target.Width / ScaleX, target.Height / ScaleY);
   
    float mapWindowX = map.Width > mapWindowSize.X
      ? Math.Clamp(_cameraCenterWorld.X - mapWindowSize.X / 2, 0, map.Width - mapWindowSize.X)
      : (map.Width - mapWindowSize.X) / 2;
    
    float mapWindowY = map.Height > mapWindowSize.Y
      ? Math.Clamp(_cameraCenterWorld.Y - mapWindowSize.Y / 2, 0, map.Height - mapWindowSize.Y)
      : (map.Height - mapWindowSize.Y) / 2;
    
    Vec2 mapWindowPos = new Vec2(mapWindowX, mapWindowY);
    Vec2Int mapWindowGridPos = MapUtils.PosToGrid(mapWindowPos);
    
    // Adjustment for smoother map scrolling 
    int adjustmentX = (int)((mapWindowGridPos.X - mapWindowPos.X) * ScaleX);
    int adjustmentY = (int)((mapWindowGridPos.Y - mapWindowPos.Y) * ScaleY);

    // Iterate over screen coordinates manually instead of calculating screen coords from map coords
    // to avoid screen tearing due to floating point rounding.
    int screenY = adjustmentY;

    // Add +1 to window sizes to compensate coord rounding 
    for (int mapOffsY = 0; mapOffsY <= mapWindowSize.Y + 1; mapOffsY++) {
      int screenX = adjustmentX;
      int mapY = mapWindowGridPos.Y + mapOffsY;

      for (int mapOffsX = 0; mapOffsX <= mapWindowSize.X + 1; mapOffsX++) {
        int mapX = mapWindowGridPos.X + mapOffsX;
        Vec2Int mapPos = MapUtils.PosToGrid(new Vec2(mapX, mapY));
        GameObject? gameObject = GetMapTile(map, mapPos.X, mapPos.Y);

        RenderGameObjectSprite(target, gameObject, screenX, screenY, timeFromStartSec);
        screenX += (int)ScaleX;
      }
      
      screenY += (int)ScaleY;
    }

    // TODO: implement proper rendering of tiled/object layers. Currently we just render all tile layers,
    //   then all object layers.
    var layers = map.GetOrderedLayers();
    
    Vec2Int viewportOffset = MapUtils.PosToGrid(new Vec2(
      mapWindowPos.X * ScaleX,
      mapWindowPos.Y * ScaleY
    ));

    for (int i = 0; i < layers.Count; i++) {
      var layer = layers[i];

      if (layer is ObjectLayer objLayer) {
        foreach (UnitObject obj in objLayer.GetObjects()) {
          int x = (int)(obj.Pos.X * ScaleX - viewportOffset.X);
          int y = (int)(obj.Pos.Y * ScaleY - viewportOffset.Y);
          int spriteWidth = obj.Sprite?.Width ?? 0;
          int spriteHeight = obj.Sprite?.Height ?? 0;

          if (x < target.Width && x + spriteWidth >= 0 && y < target.Height && y + spriteHeight >= 0) {
            RenderGameObjectSprite(target, obj, x, y, timeFromStartSec);            
          }
        }
      }
    }
  }

  private void RenderGameObjectSprite(
    ScreenBuffer target,
    GameObject? gameObject,
    int screenX,
    int screenY,
    double gameTimeSec
  ) {
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

        target.Set(x, y, cells[spriteX, spriteY]);
      }
    }
  }

  private TileObject? GetMapTile(IGameMap map, int x, int y) {
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
