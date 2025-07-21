using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Objects;

public abstract class GroundTile : TileObject {
  private int _vertDamageLevel;
  private int _horzDamageLevel;

  public int DamageLevel => _vertDamageLevel + _horzDamageLevel;
  public int VertDamageLevel => _vertDamageLevel;
  public int HorzDamageLevel => _horzDamageLevel;

  public void Visited(Direction dir) {
    if (dir is Direction.Up or Direction.Down) {
      _vertDamageLevel += 1;      
    } else {
      _horzDamageLevel += 1;      
    }
  }
}
