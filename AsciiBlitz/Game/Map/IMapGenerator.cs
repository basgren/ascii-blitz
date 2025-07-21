using AsciiBlitz.Core.Map;

namespace AsciiBlitz.Game.Map;

public interface IMapGenerator {
  IMapGenerator SetSize(int width, int height);
  GameMap Build();
}
