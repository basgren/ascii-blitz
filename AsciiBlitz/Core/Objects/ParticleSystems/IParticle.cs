using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core.Objects.ParticleSystems;

public interface IParticle {
  bool IsAlive { get; }
  char Symbol { get; }
  int Color { get; }
  Vec2 Pos { get; }
}
