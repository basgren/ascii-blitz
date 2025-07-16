namespace AsciiBlitz.Core;

public abstract class StateMachine<TState> where TState : Enum {
  private struct DelayedTransitionData {
    public TState TargetState;
    public float Delay;
    public Action<TState>? OnComplete;
  }
  
  public event Action<TState, TState>? OnBeforeStateChange;
  public event Action<TState, TState>? OnAfterStateChange;

  public TState CurrentState => _currentState;
  public float TimeInCurrentState => _timeInState;

  private TState _currentState;
  private readonly Dictionary<TState, HashSet<TState>> _transitions = new();

  private float _timeInState;

  private DelayedTransitionData? _pendingTransition;

  protected StateMachine(TState defaultState) {
    _currentState = defaultState;
    _timeInState = 0f;
    _pendingTransition = null;
  }

  public virtual void Update(float deltaTime) {
    _timeInState += deltaTime;

    if (!_pendingTransition.HasValue) {
      return;
    }

    _pendingTransition = _pendingTransition.Value with {
      Delay = _pendingTransition.Value.Delay - deltaTime
    };

    if (_pendingTransition.Value.Delay <= 0f) {
      var transition = _pendingTransition.Value;
      var from = _currentState;
      
      _pendingTransition = null;
      Go(transition.TargetState);
      transition.OnComplete?.Invoke(from);
    }
  }

  public void Initialize(TState initialState) {
    _currentState = initialState;
    _timeInState = 0f;
    _pendingTransition = null;
  }

  public void RegisterTransition(TState fromState, TState toState) {
    if (!_transitions.TryGetValue(fromState, out var set)) {
      set = new HashSet<TState>();
      _transitions[fromState] = set;
    }

    set.Add(toState);
  }

  public void RegisterTransitions(TState fromState, params TState[] toStates) {
    foreach (var toState in toStates) {
      RegisterTransition(fromState, toState);
    }
  }

  public bool CanGo(TState newState) {
    return _transitions.TryGetValue(_currentState, out var allowedStates) && allowedStates.Contains(newState);
  }

  public bool Go(TState newState) {
    if (!CanGo(newState))
      return false;

    OnBeforeStateChange?.Invoke(_currentState, newState);
    var oldState = _currentState;
    _currentState = newState;
    _timeInState = 0f;
    _pendingTransition = null;
    OnAfterStateChange?.Invoke(oldState, newState);

    return true;
  }

  public void GoDelayed(TState newState, float delaySeconds, Action<TState>? onComplete = null) {
    if (_pendingTransition.HasValue) {
      // TODO: should we auto ignore pending state when switching to another one?
      throw new InvalidOperationException("A delayed transition is already pending.");      
    }
    
    if (delaySeconds <= 0f) {
      var from = _currentState;
      Go(newState);
      onComplete?.Invoke(from);
      return;
    }

    if (!CanGo(newState)) {
      return;
    }

    _pendingTransition = new DelayedTransitionData {
      TargetState = newState,
      Delay = delaySeconds,
      OnComplete = onComplete,
    };
  }

  public void CancelDelayed() {
    _pendingTransition = null;
  }
}
