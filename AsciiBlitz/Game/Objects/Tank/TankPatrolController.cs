using AsciiBlitz.Core.Commands;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Objects.Tank;

/// <summary>
/// Very simple patrolling controller to test enemy tank movement. 
/// </summary>
public class TankPatrolController(IGameMap map) : ITankController {
  private IGameMap _map = map;

  public ITankUnitCommand? GetNextCommand(Tank unit) {
    if (unit.MovementState.State != TankMovementState.Idle) {
      return null;
    }

    // Try to move to the cell in front of the tank.
    var cellInFront = unit.Pos + unit.Dir.ToVec2();

    if (_map.IsMovable(cellInFront)) {
      return new TankMoveForwardCommand();
    }
    
    // ...if not possible, turn counterclockwise
    return new TankTurnCcwCommand();
  }
}
