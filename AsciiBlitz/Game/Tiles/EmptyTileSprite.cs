using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class EmptyTileSprite : Sprite<EmptyTile> {
  private static readonly string[] SpriteLines = [
    "   ",
    "   ",
    "   "
  ];
    
  public EmptyTileSprite() : base(SpriteLines) { }
}
