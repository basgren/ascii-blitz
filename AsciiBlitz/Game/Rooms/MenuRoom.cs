using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Services;
using AsciiBlitz.Core.Utils;
using AsciiBlitz.Game.Map;
using AsciiBlitz.Game.Rooms.Game;
using AsciiBlitz.Game.Rooms.MapGen;

namespace AsciiBlitz.Game.Rooms;

public class MenuRoom : AbstractRoom {
  private readonly IGameInput _input = Services.Get<IGameInput>();
  private readonly IConsoleRenderer _renderer = Services.Get<IConsoleRenderer>();
  private readonly IRoomService _roomService = Services.Get<IRoomService>();
  private readonly IGameState _gameState = Services.Get<IGameState>();
  private readonly DrawUtils _drawUtils;
  private bool _gameRunning = true;

  private readonly string[] _logo;
  private readonly int _logoWidth;
  
  public MenuRoom() {
    _drawUtils = new DrawUtils(_renderer.Buffer);
    _renderer.Clear();
    _logo = Assets.LoadLines("Images/BigLogo.txt");
    _logoWidth = _logo.Max(x => x.Length);
  }

  public override void OnRoomEnter() {
    _renderer.Clear();
  }

  public override bool ProcessFrame(IFrameContext frameContext) {
    var buf = _renderer.Buffer;
    var menuTop = buf.Height / 2 + 6;
    var centerX = buf.Width / 2 - 1;

    _drawUtils
      .SetTarget(buf) // Important to set, as Buffer is swapped each frame.
      .DrawTextLines(centerX - _logoWidth / 2, 0, _logo)
      .DrawTextCentered(centerX - 1, menuTop - 4, "[1] Start Random Map")
      .DrawTextCentered(centerX - 1, menuTop - 2, "[2] Generate Custom Map")
      .DrawTextCentered(centerX - 1, menuTop, "[3] Go To Test Map")
      .DrawTextCentered(centerX - 1, menuTop + 2, "[Esc] Exit");
    
    ProcessInput();

    return _gameRunning;
  }

  private void ProcessInput() {
    var key = _input.GetKey();
    bool processed = true;

    switch (key) {
      case ConsoleKey.D1:
        _gameState.InitialMazeOptions = MazeGenerator.GetRandomOptions();
        _gameState.CurrentMazeOptions = null;
        _roomService.GoToRoom<GameRoom>();
        break;
      
      case ConsoleKey.D2:
        _roomService.GoToRoom<MapGenRoom>();
        break;
      
      case ConsoleKey.D3:
        _gameState.InitialMazeOptions = null;
        _roomService.GoToRoom<GameRoom>();
        break;
      
      case ConsoleKey.Escape:
        _gameRunning = false;
        break;

      default:
        processed = false;
        break;
    }

    if (processed) {
      _input.Consume();
    }
  }
}
