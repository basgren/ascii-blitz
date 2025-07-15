using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class WallTileSprite : Sprite<WallTile> {
  private static readonly string[] SpriteLines = [
    "███",
    "███",
    "███"
  ];
    
  public WallTileSprite() : base(SpriteLines) { }
}
