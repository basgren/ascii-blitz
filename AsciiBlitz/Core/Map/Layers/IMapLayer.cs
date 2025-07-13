using AsciiBlitz.Core.Map.Objects;

namespace AsciiBlitz.Core.Map.Layers;

public interface IMapLayer {
  public void Resize(int width, int height);
  public MapObject? GetAt(int x, int y);
  public void SetAt(int x, int y, MapObject? obj);
}
