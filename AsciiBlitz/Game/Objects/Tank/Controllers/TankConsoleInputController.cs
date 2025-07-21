using AsciiBlitz.Core.Input;

namespace AsciiBlitz.Game.Objects.Tank.Controllers;

public class TankConsoleInputController(IGameInput input) : ITankController {
  public ITankUnitCommand? GetNextCommand(Tank unit) {
    ConsoleKey? key = input.GetKey();

    if (key == null
        || (unit.MovementState.State != TankMovementState.Idle && unit.WeaponState.State != TankWeaponState.Idle)
       ) {
      return null;
    }

    ITankUnitCommand? command = key switch {
      ConsoleKey.UpArrow => new TankMoveForwardCommand(),
      ConsoleKey.DownArrow => new TankMoveBackwardCommand(),
      ConsoleKey.LeftArrow => new TankTurnCcwCommand(),
      ConsoleKey.RightArrow => new TankTurnCwCommand(),
      ConsoleKey.Spacebar => new TankFireCommand(),
      _ => null, // Do nothing
    };

    if (command != null) {
      // Reset buffer, so the same key won't be retrieved in the next update.
      input.Consume();
    }

    return command;
  }
}
