using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render.Buffer;

namespace AsciiBlitz.Core.Render;

public class MiniMapRenderer {
  public void Render(ScreenBuffer target, IGameMap map) {
    // Get console size to check available space
    int consoleWidth = target.Width;
    int consoleHeight = target.Height;
    
    // Determine the actual render area (consider map size vs console size)
    int renderWidth = Math.Min(map.Width, consoleWidth);
    int renderHeight = Math.Min(map.Height, consoleHeight - 1); // Leave one line for cursor
    
    // Clear console
    Console.Clear();
    
    // Render each position
    for (int y = 0; y < renderHeight; y++) {
      for (int x = 0; x < renderWidth; x++) {
        char renderChar = GetCharForPosition(map, x, y);
        Console.SetCursorPosition(x, y);
        Console.Write(renderChar);
      }
    }
  }
  
  private char GetCharForPosition(IGameMap map, int x, int y) {
    // Check layers from highest index to lowest (back to front)
    var layers = map.GetOrderedLayers();
    
    for (int layerIndex = layers.Count - 1; layerIndex >= 0; layerIndex--) {
      var layer = layers[layerIndex];

      if (layer is TileLayer tileLayer) {
        var mapObject = tileLayer.GetTileAt(x, y);
      
        if (mapObject != null) {
          // Return character for the first non-null object found
          return GetCharForMapObject(mapObject);
        }
      }
    }
    
    // If no object found in any layer, render as empty space
    return ' ';
  }
  
  private char GetCharForMapObject(GameObject gameObject) {
    return '?';
    // return gameObject.Type switch {
    //   MapObjectType.Empty => ' ',
    //   MapObjectType.Wall => '#',
    //   _ => '?' // Unknown objects
    // };
  }
}