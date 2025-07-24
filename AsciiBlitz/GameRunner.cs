using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Loop;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Services;
using AsciiBlitz.Game;
using AsciiBlitz.Game.Map;
using AsciiBlitz.Game.Rooms;
using AsciiBlitz.Game.Rooms.MapGen;

namespace AsciiBlitz;

public class GameRunner {
  private bool _gameRunning = true;

  private readonly GameMapFactory _mapFactory = new();
  private MazeGenerationOptions _initialMazeOptions;

  // Services
  private readonly IGameInput _input;
  private readonly IRoomService _rooms;
  private readonly IConsoleRenderer _consoleRenderer;
  private readonly GameState _gameState;

  public GameRunner() {
    Services.Register<IGameInput>(new BufferedConsoleInput());
    Services.Register<IRoomService>(new RoomService());
    Services.Register<IConsoleRenderer>(new BufferedConsoleRenderer());
    
    _gameState = new GameState();
    Services.Register<IGameState>(_gameState);
    
    _input = Services.Get<IGameInput>();
    _rooms = Services.Get<IRoomService>();
    _consoleRenderer = Services.Get<IConsoleRenderer>();
  }

  public void Run() {
    Console.Clear();
    Console.CursorVisible = false;
    _consoleRenderer.SetSize(120, 30);

    _rooms.GoToRoom<IntroRoom>();

    var gameLoop = new GameLoop();


    gameLoop
      // Actually, we won't get more than 60 fps, as we use Thread.Sleep and it gives a minimal
      // delay of 15 ms, so we set target fps a bit bigger to get around 64-65 fps.
      .SetTargetFps(70)
      .SetStepFunction((frameContext) => {
        _input.Update();
        _gameState.Update(frameContext);

        _gameRunning = _rooms.CurrentRoom.ProcessFrame(frameContext);
        
        _consoleRenderer.Render();

        return _gameRunning;
      })
      .Run();
  }
}
