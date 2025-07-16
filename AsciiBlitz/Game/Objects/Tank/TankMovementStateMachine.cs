using AsciiBlitz.Core;

namespace AsciiBlitz.Game.Objects.Tank;

public enum TankMovementState {
  Idle,
  Moving,
  Turning,
}

public class TankMovementStateMachine : StateMachine<TankMovementState> {
  public TankMovementStateMachine(TankMovementState defaultState) : base(defaultState) {
    RegisterTransitions(TankMovementState.Idle, TankMovementState.Moving, TankMovementState.Turning);
    RegisterTransitions(TankMovementState.Moving, TankMovementState.Idle);
    RegisterTransitions(TankMovementState.Turning, TankMovementState.Idle);
  }
}
