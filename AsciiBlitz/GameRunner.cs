using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Loop;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Services;
using AsciiBlitz.Game;
using AsciiBlitz.Game.Rooms;

namespace AsciiBlitz;

public class GameRunner {
  // Services
  private readonly IGameInput _input;
  private readonly IRoomService _rooms;
  private readonly IConsoleRenderer _consoleRenderer;
  private readonly GameState _gameState;
  
  private bool _gameRunning = true;

  public GameRunner() {
    Services.Register<IGameInput>(new BufferedConsoleInput());
    Services.Register<IRoomService>(new RoomService());
    Services.Register<IConsoleRenderer>(new BufferedConsoleRenderer());
    
    _gameState = new GameState(); // keep implementation, not interface
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
    // _gameState.InitialMazeOptions = new MazeGenerationOptions(15, 11, 2);
    // _rooms.GoToRoom<GameRoom>();

    new GameLoop()
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
