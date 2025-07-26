using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Tiles;

public class SoilTile : GroundTile {
  public override Sprite Sprite => SpriteRepo.Get<SoilTileSprite>(); 
}
