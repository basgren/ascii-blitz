using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Render.Sprites;

public class WallSprite : Sprite<MapWall> {
  private static readonly string[] SpriteLines = [
    "███",
    "███",
    "███"
  ];
    
  public WallSprite() : base(SpriteLines) { }
}
