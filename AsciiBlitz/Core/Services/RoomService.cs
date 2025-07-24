namespace AsciiBlitz.Core.Services;

public interface IRoomService {
  IRoom CurrentRoom { get; }
  void GoToRoom<TRoom>() where TRoom : IRoom, new();
}

public interface IRoom {
  void OnRoomEnter();
  void OnRoomExit();
  bool ProcessFrame(IFrameContext frameContext);
}

public class EmptyRoom : AbstractRoom {
}

public class AbstractRoom : IRoom {
  public virtual void OnRoomEnter() {
  }

  public virtual void OnRoomExit() {
  }

  public virtual bool ProcessFrame(IFrameContext frameContext) {
    return true;
  }
}

public class RoomService : IRoomService {
  public IRoom CurrentRoom => _currentRoom;
  private IRoom _currentRoom = new EmptyRoom();

  public void GoToRoom<TRoom>() where TRoom : IRoom, new() {
    _currentRoom?.OnRoomExit();

    _currentRoom = new TRoom();
    _currentRoom.OnRoomEnter();
  }
}
