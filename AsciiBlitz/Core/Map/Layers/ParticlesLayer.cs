using AsciiBlitz.Core.Objects.ParticleSystems;

namespace AsciiBlitz.Core.Map.Layers;

public class ParticlesLayer(int id, int order) : AbstractMapLayer<ParticleSystem>(id, order) {
  private readonly List<ParticleSystem> _objects = new();

  public override void Resize(int width, int height) {
    // In this class we don't have a specific size of the layer, so do nothing here.
  }

  public override IReadOnlyList<ParticleSystem> GetObjects() {
    return _objects;
  }

  public override void Add(ParticleSystem obj) {
    _objects.Add(obj);
  }

  public override void Remove(ParticleSystem obj) {
    _objects.Remove(obj);
  }
}
