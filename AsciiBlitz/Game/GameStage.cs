using AsciiBlitz.Core;

namespace AsciiBlitz.Game;

public enum GameStage {
  LevelIntro,
  InGame,
  GameOver,
}

public class GameStageStateMachine: StateMachine<GameStage> {
  public GameStageStateMachine() : base(GameStage.LevelIntro) {
    RegisterTransition(GameStage.LevelIntro, GameStage.InGame);
    RegisterTransitions(GameStage.InGame, GameStage.GameOver, GameStage.LevelIntro);
    RegisterTransitions(GameStage.GameOver, GameStage.LevelIntro);
  }
}
