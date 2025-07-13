using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Map.Layers;

public class ObjectLayer(int id, int order) : AbstractMapLayer<MapUnitObject>(id, order) {
  private readonly List<MapUnitObject> _objects = new();

  public override void Resize(int width, int height) {
    // In this class we don't have a specific size of the layer, so do nothing here.
  }

  public override IReadOnlyList<MapUnitObject> GetObjects() {
    return _objects;
  }

  public override void Add(MapUnitObject obj) {
    _objects.Add(obj);
  }

  public override void Remove(MapUnitObject obj) {
    _objects.Remove(obj);
  }
}
