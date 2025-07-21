using AsciiBlitz.Core;
using AsciiBlitz.Core.Commands;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Objects.Tank;

public interface ITankController {
  public ITankUnitCommand? GetNextCommand(Tank unit);
}

public class TankAttrs {
  // How long it takes to turn 90deg
  public readonly float TurnTime = 0.5f;
  
  // How long it takes to move forward by 1 tile
  public readonly float MoveForwardTime = 0.5f;
  
  // How long it takes to move backward by 1 tile. Should be more than forward, but less than 2 turns + move forward.
  public readonly float MoveBackwardTime = 0.5f * 1.5f;
  
  public readonly float WeaponShotTime = 0.05f; // Just for effect of shootin in menu.
  public readonly float WeaponReloadTime = 1f;
}

public class Tank : UnitObject, ICollidable, IDamageable {
  public ITankController? Controller { get; set; }
  
  public override Sprite Sprite => SpriteRepo.Get<TankSprite>();
  public IReadonlyStateMachine<TankMovementState> MovementState => _state;
  public IReadonlyStateMachine<TankWeaponState> WeaponState => _weaponState;
  public IReadonlyStateMachine<TankDamageState> DamageState => _damageState;

  private readonly TankMovementStateMachine _state = new(TankMovementState.Idle);
  private readonly TankWeaponStateMachine _weaponState = new(TankWeaponState.Idle);
  private readonly TankDamageStateMachine _damageState = new(TankDamageState.Normal);
  private readonly TankAttrs _attrs = new();
  
  public IDamageableComponent Damageable { get; }
  
  // The next cell that tank will move to. Updated only during state = Moving.
  public Vec2 TargetPos { get; private set; } = Vec2.Zero;
  public Vec2 PrevPos { get; private set; } = Vec2.Zero;
  
  // todo: implement by inheritance.
  public bool IsPlayer = false;

  public Tank() {
    Damageable = new BaseDamageableComponent(3, OnHit);
  }
  
  public override void Update(float deltaTime) {
    base.Update(deltaTime);
    _state.Update(deltaTime);
    _weaponState.Update(deltaTime);
    _damageState.Update(deltaTime);

    if (_state.State == TankMovementState.Moving) {
      Pos = VecUtils.Mix(PrevPos, TargetPos, _state.Progress);
    }
    
    var command = Controller?.GetNextCommand(this);
    command?.Execute(this);
  }

  private void OnHit(float damage) {
    if (_damageState.Go(TankDamageState.Hit)) {
      _damageState.GoDelayed(TankDamageState.Normal, 0.15f);
    }
  }

  public RectFloat Bounds => new(Pos.X, Pos.Y, 1f, 1f);

  public bool IsActive { get; } = true;

  public void OnCollision(TileObject? tile) {
    // Do nothing.
  }

  public void OnCollision(ICollidable? tile) {
    // Do nothing
  }

  public void Fire() {
    if (_weaponState.Go(TankWeaponState.Shooting)) {
      var bullet = GameState.CreateUnit<Projectile>();
      bullet.Speed = Dir.ToVec2() * 4f;
      bullet.Pos = GetShootPoint(bullet);
      
      _weaponState.GoDelayed(TankWeaponState.Reloading, _attrs.WeaponShotTime, _ => {
        _weaponState.GoDelayed(TankWeaponState.Idle, _attrs.WeaponReloadTime);
      });
    }
  }

  public void Turn(bool counterclockwise = false) {
    if (_state.Go(TankMovementState.Turning)) {
      _state.GoDelayed(TankMovementState.Idle, _attrs.TurnTime, _ => {
        // Change tank movement direction only after turning is "finished".
        Dir = counterclockwise ? Dir.TurnCcw() : Dir.TurnCw();
      });
    }
  }

  public void MoveForward() {
    if (_state.CanGo(TankMovementState.Moving) && TryStartMove(Dir)) {
      _state.Go(TankMovementState.Moving);
    
      _state.GoDelayed(TankMovementState.Idle, _attrs.MoveForwardTime, _ => {
        EndMove();
      });        
    }
  }
  
  public void MoveBackward() {
    if (_state.CanGo(TankMovementState.Moving) && TryStartMove(Dir.Opposite())) {
      _state.Go(TankMovementState.Moving);

      _state.GoDelayed(TankMovementState.Idle, _attrs.MoveBackwardTime, _ => {
        EndMove();
      });
    }
  }

  private bool TryStartMove(Direction dir) {
    Vec2 nextPos = Pos + dir.ToVec2();

    if (!GameState.GetMap().IsMovable(nextPos)) {
      return false;
    }

    TargetPos = nextPos;
    PrevPos = Pos;
    return true;
  }

  private void EndMove() {
    Pos = TargetPos;
  }

  private Vec2 GetShootPoint(UnitObject projectile) {
    var halfSize = projectile.Size / 2;

    Vec2 offs = Dir switch {
      Direction.Up => new Vec2(0.5f - halfSize.X, -halfSize.Y * 2),
      Direction.Down => new Vec2(0.5f - halfSize.X, 1),
      Direction.Left => new Vec2(-halfSize.X * 2f, 0.5f - halfSize.Y),
      Direction.Right => new Vec2(1, 0.5f - halfSize.Y),
      _ => throw new ArgumentOutOfRangeException()
    };

    return Pos + offs;
  }
}
