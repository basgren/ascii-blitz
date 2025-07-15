using AsciiBlitz.Core.Objects;

namespace AsciiBlitz.Core.Render.Sprites;

public class UnknownSprite : Sprite<GameObject> {
  private static readonly string[] SpriteLines = {
    "???",
    "???",
    "???"
  };
    
  public UnknownSprite() : base(SpriteLines) { }
}
