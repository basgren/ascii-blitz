using AsciiBlitz.Core.Types;
using AsciiBlitz.Game.Objects.ParticleSystems;

namespace AsciiBlitz.Core.Objects.ParticleSystems;

public class ParticleSystem : GameObject {
  public IReadOnlyList<IParticle> Particles => _particles;
  public Vec2 Pos { get; protected set; }

  protected readonly List<TankExplosionParticle> _particles = new();
  protected float _duration = 1f;
  protected float _elapsed;

  public virtual void SetPos(Vec2 pos) {
    Pos = pos;
  }

  public override void Update(float dt) {
    _elapsed += dt;

    foreach (var p in _particles) {
      p.Update(dt);
    }

    if (_elapsed >= _duration) {
      Destroy();
    }
  }
}
