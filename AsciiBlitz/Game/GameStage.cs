using AsciiBlitz.Core;

namespace AsciiBlitz.Game;

public enum GameStage {
  LevelIntro,
  InGame,
  LevelComplete, // Just a pause before next level
  GameOver,
}

public class GameStageStateMachine: StateMachine<GameStage> {
  public GameStageStateMachine() : base(GameStage.LevelIntro) {
    RegisterTransition(GameStage.LevelIntro, GameStage.InGame);
    RegisterTransitions(GameStage.InGame, GameStage.GameOver, GameStage.LevelComplete);
    RegisterTransitions(GameStage.LevelComplete, GameStage.LevelIntro);
    RegisterTransitions(GameStage.GameOver, GameStage.LevelIntro);
  }
}
