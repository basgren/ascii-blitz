using AsciiBlitz.Core.Render.Sprites;

namespace AsciiBlitz.Game.Objects;

public class ProjectileSprite() : Sprite<Projectile>(SpriteLines) {
  public override int Width => 1;
  public override int Height => 1;

  private static readonly string[] SpriteLines = [
    "*",
  ];
}
