namespace AsciiBlitz.Core;

public interface IReadonlyStateMachine<TState> where TState : Enum {
  TState State { get; }
  float TimeInCurrentState { get; }
  float Progress { get; }
}

/// <summary>
/// Base class for a finite state machine (FSM) with delayed transitions.
/// </summary>
public abstract class StateMachine<TState>(TState defaultState) : IReadonlyStateMachine<TState> where TState : Enum {
  /// <summary>
  /// Struct describing a delayed state transition.
  /// </summary>
  private record struct DelayedTransitionData
  (
    TState TargetState,
    float Delay,
    float TotalDelay,
    Action<TState>? OnComplete);

  private TState _state = defaultState;
  private readonly Dictionary<TState, HashSet<TState>> _transitions = new();
  private float _timeInState;
  private DelayedTransitionData? _pendingTransition;

  /// <summary>
  /// Invoked before a state transition occurs.
  /// </summary>
  public event Action<TState, TState>? OnBeforeStateChange;

  /// <summary>
  /// Invoked after a state transition completes.
  /// </summary>
  public event Action<TState, TState>? OnAfterStateChange;

  /// <summary>
  /// The currently active state.
  /// </summary>
  public TState State => _state;

  /// <summary>
  /// Time spent in the current state.
  /// </summary>
  public float TimeInCurrentState => _timeInState;

  /// <summary>
  /// State progress: returns 1.0 if no delayed transition is pending, otherwise returns fraction of delay elapsed.
  /// </summary>
  public float Progress {
    get {
      if (_pendingTransition is { TotalDelay: > 0f } pending) {
        float elapsed = pending.TotalDelay - pending.Delay;
        return Math.Clamp(elapsed / pending.TotalDelay, 0f, 1f);
      }

      return 1f;
    }
  }

  /// <summary>
  /// Updates internal timers and executes any delayed transition.
  /// </summary>
  public virtual void Update(float deltaTime) {
    _timeInState += deltaTime;

    if (_pendingTransition is { } pending) {
      var newDelay = pending.Delay - deltaTime;

      if (newDelay <= 0f) {
        _pendingTransition = null;
        var from = _state;
        Go(pending.TargetState);
        pending.OnComplete?.Invoke(from);
      } else {
        _pendingTransition = pending with { Delay = newDelay };
      }
    }
  }

  /// <summary>
  /// Registers a valid transition between two states.
  /// </summary>
  public void RegisterTransition(TState fromState, TState toState) {
    if (!_transitions.TryGetValue(fromState, out var set)) {
      set = new HashSet<TState>();
      _transitions[fromState] = set;
    }

    set.Add(toState);
  }

  /// <summary>
  /// Registers multiple valid transitions from a single source state.
  /// </summary>
  public void RegisterTransitions(TState fromState, params TState[] toStates) {
    foreach (var toState in toStates) {
      RegisterTransition(fromState, toState);
    }
  }

  /// <summary>
  /// Checks if the FSM can transition to the given state from the current state.
  /// </summary>
  public bool CanGo(TState newState) {
    return _transitions.TryGetValue(_state, out var allowedStates)
           && allowedStates.Contains(newState);
  }

  /// <summary>
  /// Transitions to a new state immediately, if allowed.
  /// </summary>
  public bool Go(TState newState) {
    if (!CanGo(newState)) {
      return false;
    }

    OnBeforeStateChange?.Invoke(_state, newState);

    var oldState = _state;
    _state = newState;
    _timeInState = 0f;
    _pendingTransition = null;

    OnAfterStateChange?.Invoke(oldState, newState);
    return true;
  }

  /// <summary>
  /// Schedules a transition to a new state after a delay.
  /// </summary>
  public void GoDelayed(TState newState, float delaySeconds, Action<TState>? onComplete = null) {
    if (_pendingTransition.HasValue) {
      throw new InvalidOperationException("A delayed transition is already pending.");
    }

    if (delaySeconds <= 0f) {
      var from = _state;
      Go(newState);
      onComplete?.Invoke(from);
      return;
    }

    if (!CanGo(newState)) {
      return;
    }

    _pendingTransition = new DelayedTransitionData(
      TargetState: newState,
      Delay: delaySeconds,
      TotalDelay: delaySeconds,
      OnComplete: onComplete
    );
  }

  /// <summary>
  /// Cancels any currently scheduled delayed transition.
  /// </summary>
  public void CancelDelayed() {
    _pendingTransition = null;
  }
}
