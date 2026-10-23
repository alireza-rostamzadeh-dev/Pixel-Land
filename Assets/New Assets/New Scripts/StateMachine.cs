
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class StateMachine<TEntity>
{
    private readonly Dictionary<string, List<StateTransition<TEntity>>> transitions =
        new Dictionary<string, List<StateTransition<TEntity>>>();

    private readonly List<State<TEntity>> states = new List<State<TEntity>>();

    public State<TEntity> CurrentState { get; private set; }

    private bool isStarted;

    public void AddState(State<TEntity> state)
    {
        if (state == null)
            throw new ArgumentNullException(nameof(state));

        if (states.Contains(state))
            return;

        states.Add(state);

        if (CurrentState == null)
            CurrentState = state;
    }

    public void AddTransition(
        State<TEntity> from,
        State<TEntity> to,
        string message,
        Func<bool> condition = null)
    {
        if (from == null)
            throw new ArgumentNullException(nameof(from));

        if (to == null)
            throw new ArgumentNullException(nameof(to));

        if (string.IsNullOrEmpty(message))
            throw new ArgumentException(
                "Transition message cannot be empty.",
                nameof(message));

        if (!states.Contains(from))
            AddState(from);

        if (!states.Contains(to))
            AddState(to);

        if (!transitions.TryGetValue(message, out List<StateTransition<TEntity>> list))
        {
            list = new List<StateTransition<TEntity>>();
            transitions.Add(message, list);
        }

        list.Add(new StateTransition<TEntity>(from, to, message, condition));
    }

    public void Start()
    {
        if (isStarted || CurrentState == null)
            return;

        isStarted = true;
        CurrentState.Enter();
    }

    public bool SendMessage(string message)
    {
        if (!isStarted || CurrentState == null ||
            string.IsNullOrEmpty(message))
            return false;

        if (!transitions.TryGetValue(message, out List<StateTransition<TEntity>> list))
            return false;

        for (int i = 0; i < list.Count; i++)
        {
            StateTransition<TEntity> transition = list[i];

            if (CurrentState != transition.FromState)
                continue;

            if (transition.Condition != null && !transition.Condition())
                continue;

            ChangeState(transition.ToState);
            return true;
        }

        return false;
    }

    private void ChangeState(State<TEntity> nextState)
    {
        if (nextState == null || CurrentState == nextState)
            return;

        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Update()
    {
        if (isStarted && CurrentState != null)
            CurrentState.Update();
    }

    public void FixedUpdate()
    {
        if (isStarted && CurrentState != null)
            CurrentState.FixedUpdate();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStarted && CurrentState != null)
            CurrentState.OnCollisionEnter2D(collision);
    }
}

public abstract class State<TEntity>
{
    protected readonly TEntity entity;
    protected readonly StateMachine<TEntity> stateMachine;

    protected State(
        TEntity entity,
        StateMachine<TEntity> stateMachine)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void Exit() { }

    public virtual void OnCollisionEnter2D(Collision2D collision) { }
}

public sealed class StateTransition<TEntity>
{
    public State<TEntity> FromState { get; private set; }

    public State<TEntity> ToState { get; private set; }

    public string Message { get; private set; }

    public Func<bool> Condition { get; private set; }

    public StateTransition(
        State<TEntity> from,
        State<TEntity> to,
        string message,
        Func<bool> condition = null)
    {
        FromState = from;
        ToState = to;
        Message = message;
        Condition = condition;
    }
}
