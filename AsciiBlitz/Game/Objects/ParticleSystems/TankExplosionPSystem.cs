using AsciiBlitz.Core.Objects.ParticleSystems;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Game.Objects.ParticleSystems;

public class TankExplosionPSystem : ParticleSystem {
  const float TileWidth = 5;
  const float TileHeight = 3;
  
  private ScreenCell[,]? _cells;

  public TankExplosionPSystem() {
    _duration = 1f;
  }

  public override void SetPos(Vec2 pos) {
    base.SetPos(pos);

    float width = _cells?.GetLength(0) ?? TileWidth;
    float height = _cells?.GetLength(1) ?? TileHeight;

    var center = pos + new Vec2(0.5f, 0.5f);
    
    // Offset to have more even movement over all directions.
    var offs = new Vec2(1 / width / 2, 1 / height / 2);
    
    for (float dy = 0; dy < 3; dy++) {
      for (float dx = 0; dx < 5; dx++) {
        var from = pos + new Vec2(dx / width, dy / height) + offs;
        var dir = (from - center).Normalized();
        var speed = 1f + 0.75f * Random.Shared.NextSingle(); // Radial speed
        var velocity = dir * speed;

        _particles.Add(new TankExplosionParticle(from, velocity, _cells?[(int)dx, (int)dy].Char ?? 'O'));
      }
    }
  }

  public void SetParticleChars(ScreenCell[,] cells) {
    _cells = cells;
  }
}

public class TankExplosionParticle(Vec2 pos, Vec2 velocity, char initChar) : IParticle, IUpdatable {
  public Vec2 Pos => _pos;
  private readonly float _lifetime = 1f;
  private float _elapsed;

  private Vec2 _pos = pos;
  
  public char Symbol {
    get {
      float t = _elapsed / _lifetime;
      if (t < 0.5f) return initChar;
      if (t < 0.75f) return 'o';
      return '.';
    }
  }

  public int Color {
    get {
      float t = _elapsed / _lifetime;

      if (t < 0.5f) return AnsiColor.Red;
      if (t < 0.75f) return AnsiColor.Yellow;
      return AnsiColor.Grayscale(8);
    }
  }

  public bool IsAlive => _elapsed < _lifetime;

  public void Update(float deltaTime) {
    _elapsed += deltaTime;
    _pos += velocity * deltaTime;
  }
}
