namespace AsciiBlitz;

public delegate bool FrameStepFunction(IFrameContext deltaTime);

public class GameLoop {
  private float _targetFps = 60;
  private FrameStepFunction? _stepFunction;
  private FrameStats? _frameStats;

  public GameLoop SetTargetFps(float fps) {
    _targetFps = fps;
    _frameStats = new FrameStats(_targetFps);
    return this;
  }

  public GameLoop SetStepFunction(FrameStepFunction stepFunction) {
    _stepFunction = stepFunction;
    return this;
  }

  public void Run() {
    if (_stepFunction == null) {
      throw new Exception("Step function is not set");
    }

    _frameStats = new FrameStats(_targetFps);
    
    bool gameRunning = true;

    while (gameRunning) {
      _frameStats.FrameUpdateStarted();
      gameRunning = _stepFunction(_frameStats);
      _frameStats.FrameUpdateFinished();

      // Frame rate limiting. Actually running Thread.Sleep with small amounts will lead to
      // pause in ~15ms. So minor adjustments are done to be closer to target FPS
      int remainingTimeMs = (int)(_frameStats.RemainingFrameTime * 1000); 
      if (remainingTimeMs > 2) {
        Thread.Sleep(remainingTimeMs - 1);
      }
    }
  }
}
