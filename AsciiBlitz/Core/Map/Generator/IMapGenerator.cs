namespace AsciiBlitz.Core.Map.Generator;

public interface IMapGenerator {
  IMapGenerator SetSize(int width, int height);
  GameMap Build();
}
