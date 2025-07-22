using AsciiBlitz.Core.Map;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Objects.Tank.Controllers;

public class TankRandomWalkController(IGameMap map, Tank player, int? seed = null) : ITankController {
  private readonly Random _rng = seed.HasValue ? new Random(seed.Value) : new Random();

  public ITankUnitCommand? GetNextCommand(Tank unit) {
    if (unit.MovementState.State != TankMovementState.Idle) {
      return null;
    }

    // Shoot player if on sight
    if (IsPlayerInSight(unit) && unit.WeaponState.State == TankWeaponState.Idle) {
      return new TankFireCommand();
    }

    // Try to move forward
    var nextPos = unit.Pos + unit.Dir.ToVec2();

    if (map.IsMovable(nextPos) && !map.IsColliding(unit, nextPos)) {
      return new TankMoveForwardCommand();
    }

    // If obstacle in front of tank, pick new random direction
    var directions = Enum.GetValues<Direction>().OrderBy(_ => _rng.Next()).ToList();

    foreach (var dir in directions) {
      var target = unit.Pos + dir.ToVec2();
      if (map.IsMovable(target) && !map.IsColliding(unit, target)) {
        if (dir != unit.Dir) {
          return TurnCommand(unit.Dir, dir);
        }

        return new TankMoveForwardCommand();
      }
    }

    return null;
  }

  private bool IsPlayerInSight(Tank unit) {
    Vec2Int direction = unit.Dir.ToVec2Int();
    Vec2Int from = MapUtils.PosToGrid(unit.Pos);
    Vec2Int to = MapUtils.PosToGrid(player.Pos);

    // Check that player is on the same x or y line
    Vec2Int delta = to - from;

    // Check that the direction to player is the same as direction of enemy unit
    if ((direction.X != 0 && delta.Y != 0) || (direction.Y != 0 && delta.X != 0)) {
      return false;
    }

    for (int i = 1; i <= unit.FireRange; i++) {
      from += direction;
      
      if (!map.IsVisionTransparent(from)) {
        return false;
      }
      
      if (from == to) {
        return true;
      }
    }

    return false;
  }

  private ITankUnitCommand TurnCommand(Direction from, Direction to) {
    if (from.TurnCw() == to) {
      return new TankTurnCwCommand();
    }

    if (from.TurnCcw() == to) {
      return new TankTurnCcwCommand();
    }

    return _rng.Next(2) == 0 ? new TankTurnCwCommand() : new TankTurnCcwCommand();
  }
}
