using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Render.Sprites;

public class EmptySprite : Sprite<EmptyTile> {
  private static readonly string[] SpriteLines = [
    "   ",
    "   ",
    "   "
  ];
    
  public EmptySprite() : base(SpriteLines) { }
}
