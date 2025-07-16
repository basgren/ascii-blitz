using AsciiBlitz.Core.Commands;
using AsciiBlitz.Core.Map;
using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Types;

namespace AsciiBlitz.Game.Objects;

public interface ITankController {
  public ITankUnitCommand? GetNextCommand(Tank unit);
}

public class TankAttrs {
  // How long it takes to turn 90deg
  public readonly float TurnTime = 0.75f;
  
  // How long it takes to move forward by 1 tile
  public readonly float MoveForwardTime = 0.75f;
  
  // How long it takes to move backward by 1 tile. Should be more than forward, but less than 2 turns + move forward.
  public readonly float MoveBackwardTime = 0.75f * 1.5f;
}

public class Tank : UnitObject, ICollidable, IDamageable {
  public ITankController? Controller { get; set; }
  
  public override Sprite Sprite => SpriteRepo.Get<TankSprite>();

  private TankMovementStateMachine state = new(TankMovementState.Idle);
  private TankAttrs _attrs = new();
  
  // The next cell that tank will move to. Updated only during state = Moving. This also can be used
  // to check for cell availability for other units.
  public Vec2 TargetPos { get; private set; } = Vec2.Zero;
  public Vec2 PrevPos { get; private set; } = Vec2.Zero;

  public override void Update(float deltaTime) {
    base.Update(deltaTime);
    state.Update(deltaTime);

    if (state.State == TankMovementState.Moving) {
      Pos = VecUtils.Mix(PrevPos, TargetPos, state.StateProgress);
    }
    
    var command = Controller?.GetNextCommand(this);
    command?.Execute(this);
  }

  public IDamageableComponent Damageable { get; } = new BaseDamageableComponent(3);
  public RectFloat Bounds => new RectFloat(Pos.X, Pos.Y, 1f, 1f);

  public bool IsActive { get; } = true;

  public void OnCollision(TileObject? tile) {
    // Do nothing.
  }

  public void OnCollision(ICollidable? tile) {
    // Do nothing
  }

  public void Fire() {
    var bullet = GameState.CreateUnit<Projectile>();
    bullet.Speed = VecUtils.DirToVec2(Dir) * 3f;
    bullet.Pos = GetShootPoint();
  }

  public void Turn(bool counterclockwise = false) {
    if (state.CanGo(TankMovementState.Turning)) {
      state.Go(TankMovementState.Turning);
      state.GoDelayed(TankMovementState.Idle, _attrs.TurnTime, _ => {
        // Change tank movement direction only after turning is "finished".
        Dir = counterclockwise ? Dir.TurnCcw() : Dir.TurnCw();
      });
    }
  }

  public void MoveForward() {
    if (state.CanGo(TankMovementState.Moving) && TryStartMove(Dir)) {
      state.Go(TankMovementState.Moving);
    
      state.GoDelayed(TankMovementState.Idle, _attrs.MoveForwardTime, _ => {
        EndMove();
      });        
    }
  }
  
  public void MoveBackward() {
    if (state.CanGo(TankMovementState.Moving) && TryStartMove(Dir.Opposite())) {
      state.Go(TankMovementState.Moving);

      state.GoDelayed(TankMovementState.Idle, _attrs.MoveBackwardTime, _ => {
        EndMove();
      });
    }
  }

  private bool TryStartMove(Direction dir) {
    Vec2 nextPos = Pos + dir.ToVec2();

    if (!CanMove(nextPos)) {
      return false;
    }

    TargetPos = nextPos;
    PrevPos = Pos;
    return true;
  }

  private void EndMove() {
    Pos = TargetPos;
  }
  
  private void TryMove(Direction dir) {
    Vec2 offs = dir.ToVec2();

    if (CanMove(Pos + offs)) {
      var newPos = Pos + offs;
      var oldGridPos = MapUtils.PosToGrid(Pos);
      var newGridPos = MapUtils.PosToGrid(newPos);
      
      Pos = newPos;

      // TODO: implement grass damage in separate system.
      // if (oldGridPos != newGridPos) {
      //   GameObject? obj = GameState.GetMap().GetLayer<TileLayer>(GameMap.LayerGroundId).GetTileAt(Pos);
      //   obj?.Visited();        
      // }
    }
  }

  private bool CanMove(Vec2 cellPos) {
    TileLayer layer = GameState.GetMap().GetLayer<TileLayer>(GameMap.LayerSolidsId);
    return !layer.HasTileAt(cellPos);
  }

  private Vec2 GetShootPoint() {
    // TODO: actually we should spawn bullet farther from tank barrel, but we should take into account that
    //   bullet is 1x1 char size in final view. but it means it has actual width = 1 / 3 and height = 1 / 3,
    //   as all objects are multiplied by 3 when displayed. In general we should take collision rect and use
    //   it here in calculations.
    int viewScale = 3; // temporary solution

    Vec2 offs = Dir switch {
      Direction.Up => new Vec2(0.5f, -1f / viewScale),
      Direction.Down => new Vec2(0.5f, 1),
      Direction.Left => new Vec2(-1f / viewScale, 0.5f),
      Direction.Right => new Vec2(1, 0.5f),
      _ => throw new ArgumentOutOfRangeException()
    };

    return Pos + offs;
  }
}
