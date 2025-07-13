using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Render.Sprites;

public class UnknownSprite : Sprite<MapObject> {
  private static readonly string[] SpriteLines = {
    "???",
    "???",
    "???"
  };
    
  public UnknownSprite() : base(SpriteLines) { }
}
