using AsciiBlitz.Core;

namespace AsciiBlitz.Game.Objects.Tank;

public enum TankWeaponState {
  Idle,
  Shooting,
  Reloading,
}

public class TankWeaponStateMachine : StateMachine<TankWeaponState> {
  public TankWeaponStateMachine(TankWeaponState defaultState) : base(defaultState) {
    RegisterTransitions(TankWeaponState.Idle, TankWeaponState.Shooting);
    RegisterTransitions(TankWeaponState.Shooting, TankWeaponState.Reloading);
    RegisterTransitions(TankWeaponState.Reloading, TankWeaponState.Idle);
  }
}
