using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<TSelf, TInitialState> : MonoBehaviour
    where TSelf : MonoBehaviour
    where TInitialState : StateMachine<TSelf, TInitialState>.State, new()
{
    public State CurrentState { get; private set; }

    readonly Dictionary<System.Type, State> states = new();

    public virtual void Start()
    {
        GoToState<TInitialState>();
    }

    public virtual void Update()
    {
        CurrentState?.UpdateState();
    }

    /// <summary>
    /// Throws if the current state is not of type TState.
    /// </summary>
    public void AssertState<TState>()
        where TState : State
    {
        Debug.Assert(
            CurrentState is TState,
            $"Encountered incorrect state! Got {CurrentState.GetType().Name}, expected {typeof(TState).Name}."
        );
    }

    public void GoToState<TState>()
        where TState : State, new()
    {
        // Create state instance if none exists
        if (!states.TryGetValue(typeof(TState), out State newState))
        {
            newState = new TState { Self = (TSelf)(object)this };
            states[typeof(TState)] = newState;
        }

        // Debug message
        if (CurrentState is null)
        {
            Debug.Log($"Entering {typeof(TState).Name}");
        }
        else
        {
            Debug.Log($"Entering {typeof(TState).Name} (from {CurrentState.GetType().Name})");
        }

        CurrentState?.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();

        // Auto-refresh UI after state change
        GameUI.Instance.Refresh();
    }

    public abstract class State
    {
        public TSelf Self { get; set; }

        public virtual void EnterState() { }

        public virtual void UpdateState() { }

        public virtual void ExitState() { }
    }
}
