namespace AsciiBlitz.Game.Map;

public static class MapSymbols {
  public const char Wall = '#';
  public const char WeakWall = '%';
  public const char Empty = ' ';
  public const char Grass = 'g';
  public const char Wheat = 'w';
  public const char River = '~';
  public const char Bridge = 'B';
  public const char Player = 'P';
  public const char Exit = 'E';
  public const char Enemy = 'T';

  public static readonly char[] Walkable = { Empty, Grass, Wheat, Bridge, Player, Exit, Enemy, WeakWall };
}
