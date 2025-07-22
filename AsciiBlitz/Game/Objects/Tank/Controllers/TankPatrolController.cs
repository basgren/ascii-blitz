using AsciiBlitz.Core.Map;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Objects.Tank.Controllers;

/// <summary>
/// Very simple patrolling controller to test enemy tank movement. 
/// </summary>
public class TankPatrolController(IGameMap map) : ITankController {
  public ITankUnitCommand? GetNextCommand(Tank unit) {
    if (unit.MovementState.State != TankMovementState.Idle) {
      return null;
    }

    // Try to move to the cell in front of the tank.
    var cellInFront = unit.Pos + unit.Dir.ToVec2();

    if (map.IsMovable(cellInFront) && !map.IsColliding(unit, cellInFront)) {
      return new TankMoveForwardCommand();
    }
    
    // ...if not possible, turn counterclockwise
    return new TankTurnCcwCommand();
  }
}
