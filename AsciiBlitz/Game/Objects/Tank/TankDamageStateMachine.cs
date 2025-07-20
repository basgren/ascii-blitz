using AsciiBlitz.Core;

namespace AsciiBlitz.Game.Objects.Tank;

public enum TankDamageState {
  Normal,
  Hit,
}

public class TankDamageStateMachine : StateMachine<TankDamageState> {
  public TankDamageStateMachine(TankDamageState defaultState) : base(defaultState) {
    RegisterTransitions(TankDamageState.Normal, TankDamageState.Hit);
    RegisterTransitions(TankDamageState.Hit, TankDamageState.Normal);
  }
}
