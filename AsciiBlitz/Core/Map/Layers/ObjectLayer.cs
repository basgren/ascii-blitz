using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Objects;

namespace AsciiBlitz.Core.Map.Layers;

public class ObjectLayer(int id, int order) : AbstractMapLayer<UnitObject>(id, order) {
  private readonly List<UnitObject> _objects = new();

  public override void Resize(int width, int height) {
    // In this class we don't have a specific size of the layer, so do nothing here.
  }

  public override IReadOnlyList<UnitObject> GetObjects() {
    return _objects;
  }

  public override void Add(UnitObject obj) {
    _objects.Add(obj);
  }

  public override void Remove(UnitObject obj) {
    _objects.Remove(obj);
  }
}
