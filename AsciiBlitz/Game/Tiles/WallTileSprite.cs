using AsciiBlitz.Core.Render;

namespace AsciiBlitz.Game.Tiles;

public class WallTileSprite() : StaticSprite([
  "█████",
  "█████",
  "█████",
], BaseColor) {
  public static readonly int BaseColor = AnsiColor.Grayscale(10);
}
