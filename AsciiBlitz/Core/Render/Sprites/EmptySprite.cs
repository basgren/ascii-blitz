using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Render.Sprites;

public class EmptySprite : Sprite<MapEmpty> {
  private static readonly string[] SpriteLines = [
    "   ",
    "   ",
    "   "
  ];
    
  public EmptySprite() : base(SpriteLines) { }
}
