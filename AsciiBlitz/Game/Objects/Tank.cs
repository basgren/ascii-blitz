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
  public float TurnTime = 0.75f;
  
  // How long it takes to move forward by 1 tile
  public float MoveForwardTime = 0.75f;
  
  // How long it takes to move backward by 1 tile. Should be more than forward, but less than 2 turns + move forward.
  public float MoveBackwardTime = 0.75f * 1.5f;
}

public class Tank : UnitObject, ICollidable, IDamageable {
  public ITankController? Controller { get; set; }
  
  public override Sprite Sprite => SpriteRepo.Get<TankSprite>();

  private TankMovementStateMachine state = new(TankMovementState.Idle);
  private TankAttrs _attrs = new();

  public override void Update(float deltaTime) {
    base.Update(deltaTime);
    state.Update(deltaTime);
    
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
    if (state.CurrentState == TankMovementState.Idle) {
      state.Go(TankMovementState.Turning);
      state.GoDelayed(TankMovementState.Idle, _attrs.TurnTime, _ => {
        // Change tank movement direction only after turning is "finished".
        Dir = counterclockwise ? Dir.TurnCcw() : Dir.TurnCw();
      });
    }
  }

  public void MoveForward() {
    if (state.CurrentState == TankMovementState.Idle) {
      state.Go(TankMovementState.Moving);
      state.GoDelayed(TankMovementState.Idle, _attrs.MoveForwardTime, _ => {
        TryMove(Dir);
      });
    }
  }
  
  public void MoveBackward() {
    if (state.CurrentState == TankMovementState.Idle) {
      state.Go(TankMovementState.Moving);
      state.GoDelayed(TankMovementState.Idle, _attrs.MoveBackwardTime, _ => {
        TryMove(Dir.Opposite());
      });
    }
  }
  
  private void TryMove(Direction dir) {
    Vec2 offs = dir.ToVec2();

    if (!offs.IsZero && CanMove(Pos, offs)) {
      var newPos = Pos + offs;
      var oldGridPos = MapUtils.PosToGrid(Pos);
      var newGridPos = MapUtils.PosToGrid(newPos);
      
      Pos = newPos;

      if (oldGridPos != newGridPos) {
        GameObject? obj = GameState.GetMap().GetLayer<TileLayer>(GameMap.LayerGroundId).GetTileAt(Pos);
        obj?.Visited();        
      }
    }
  }

  private bool CanMove(Vec2 playerPos, Vec2 offs) {
    TileLayer layer = GameState.GetMap().GetLayer<TileLayer>(GameMap.LayerSolidsId);
    Vec2 newPos = playerPos + offs;
    
    return !layer.HasTileAt(newPos);
  }
  
  public Vec2 GetShootPoint() {
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
