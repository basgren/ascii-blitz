namespace AsciiBlitz.Game.Objects.Tank;

public interface ITankUnitCommand {
  void Execute(Tank unit);
}

public readonly struct TankMoveForwardCommand : ITankUnitCommand {
  public void Execute(Tank unit) {
    unit.MoveForward();
  }
}

public readonly struct TankMoveBackwardCommand : ITankUnitCommand {
  public void Execute(Tank unit) {
    unit.MoveBackward();
  }
}

public readonly struct TankTurnCwCommand : ITankUnitCommand {
  public void Execute(Tank unit) {
    unit.Turn();
  }
}

public readonly struct TankTurnCcwCommand : ITankUnitCommand {
  public void Execute(Tank unit) {
    unit.Turn(true);
  }
}

public readonly struct TankFireCommand : ITankUnitCommand {
  public void Execute(Tank unit) {
    unit.Fire();
  }
}
