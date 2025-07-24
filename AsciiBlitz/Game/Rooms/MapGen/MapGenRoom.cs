using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Services;
using AsciiBlitz.Game.Map;
using AsciiBlitz.Game.Rooms.Game;

namespace AsciiBlitz.Game.Rooms.MapGen;

public class MapGenRoom : AbstractRoom {
  private readonly IGameState _gameState = Services.Get<IGameState>();
  private readonly IConsoleRenderer _renderer = Services.Get<IConsoleRenderer>();
  private readonly IGameInput _input = Services.Get<IGameInput>();
  private readonly IRoomService _roomService = Services.Get<IRoomService>();
  private readonly MapGenScreen _screen = new();
  private readonly MapGenContext _mapGenContext = new();

  public override void OnRoomEnter() {
    _mapGenContext.Options = _gameState.InitialMazeOptions ?? new MazeGenerationOptions(41, 41, 7888);
  }

  public override bool ProcessFrame(IFrameContext frameContext) {
    ProcessInput();

    _screen.Render(_renderer.Buffer, _mapGenContext, frameContext);
    
    if (_mapGenContext.Map == null) {
      _mapGenContext.Map = MazeGenerator.GenerateValidMaze(_mapGenContext.Options);
    }

    return true;
  }

  private void OptionsChanged() {
    MazeGenerator.ClampGenerationOptions(_mapGenContext.Options);;

    _mapGenContext.OffsX = 0;
    _mapGenContext.OffsY = 0;
    _mapGenContext.Map = null;
  }
  
  private void ProcessInput() {
    var key = _input.GetKey();
    
    bool processed = true;
    
    switch (key) {
      case ConsoleKey.UpArrow:
        _mapGenContext.OffsY++;
        ClampOffset();
        break;

      case ConsoleKey.DownArrow:
        _mapGenContext.OffsY--;
        ClampOffset();
        break;

      case ConsoleKey.LeftArrow:
        _mapGenContext.OffsX++;
        ClampOffset();
        break;

      case ConsoleKey.RightArrow:
        _mapGenContext.OffsX--;
        ClampOffset();
        break;
      
      case ConsoleKey.W:
        _mapGenContext.Options.Height += 2;
        OptionsChanged();
        break;
      
      case ConsoleKey.S:
        _mapGenContext.Options.Height -= 2;
        OptionsChanged();
        break;
      
      case ConsoleKey.A:
        _mapGenContext.Options.Width -= 2;
        OptionsChanged();
        break;
      
      case ConsoleKey.D:
        _mapGenContext.Options.Width += 2;
        OptionsChanged();
        break;
      
      case ConsoleKey.Q:
        _mapGenContext.Options.Seed--;
        OptionsChanged();
        break;
      
      case ConsoleKey.E:
        _mapGenContext.Options.Seed++;
        OptionsChanged();
        break;
      
      case ConsoleKey.R:
        _mapGenContext.Options = MazeGenerator.GetRandomOptions();
        OptionsChanged();
        break;
      
      case ConsoleKey.Enter:
        _gameState.InitialMazeOptions = _mapGenContext.Options;
        _gameState.CurrentMazeOptions = null;
        _roomService.GoToRoom<GameRoom>();
        break;

      case ConsoleKey.Escape:
        _roomService.GoToRoom<MenuRoom>();
        break;

      default:
        processed = false;
        break;
    }

    if (processed) {
      _input.Consume();
    }
  }

  private void ClampOffset() {
    int ClampAxis(int mapSize, int viewSize, int current) {
      if (mapSize <= viewSize) return 0;

      int halfDiff = (mapSize - viewSize) / 2;
      return Math.Clamp(current, -halfDiff - 1, halfDiff);
    }

    _mapGenContext.OffsX = ClampAxis(_screen.MapBuffer.Width, _screen.GameViewport.Width, _mapGenContext.OffsX);
    _mapGenContext.OffsY = ClampAxis(_screen.MapBuffer.Height, _screen.GameViewport.Height, _mapGenContext.OffsY);
  }
}
