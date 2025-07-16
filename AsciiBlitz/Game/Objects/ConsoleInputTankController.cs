using AsciiBlitz.Core.Commands;
using AsciiBlitz.Core.Input;

namespace AsciiBlitz.Game.Objects;

public class ConsoleInputTankController : ITankController {
  private IGameInput _input;
  
  public ConsoleInputTankController(IGameInput gameInput) {
    _input = gameInput;
  }
  
  public ITankUnitCommand? GetNextCommand(Tank unit) {
    ConsoleKey? key = _input.GetKey();

    if (key == null) {
      return null;
    }

    return key switch {
      ConsoleKey.UpArrow => new TankMoveForwardCommand(),
      ConsoleKey.DownArrow => new TankMoveBackwardCommand(),
      ConsoleKey.LeftArrow => new TankTurnCcwCommand(),
      ConsoleKey.RightArrow => new TankTurnCwCommand(),
      ConsoleKey.Spacebar => new TankFireCommand(),
      _ => null, // Do nothing
    };
  }
}
