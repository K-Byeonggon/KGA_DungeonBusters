using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine
{
    private GameState currentState;
    private Dictionary<GameState, IState> states;

    //GameStateMachine밖에서 이벤트를 구독해서 State가 변경되었을때 로직 실행.
    public event Action<GameState> OnStateChanged;

    public GameStateMachine()
    {
        states = new Dictionary<GameState, IState>();
        
    }

    private void InitializeStates()
    {
        states[GameState.StartDungeon] = new StartDungeonState(this);
    }

    public void ChangeState(GameState newState)
    {
        if(currentState == newState) return;

        Debug.Log($"ChangeState from <color=yellow>{currentState}</color> " +
                              $"to <color=red>{newState}</color>");

        currentState = newState;

        if(states.ContainsKey(currentState))
        {
            states[currentState].OnEnter();
        }

        OnStateChanged?.Invoke(currentState);

    }

    #region IState
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }

    public class StartDungeonState : IState
    {
        private GameStateMachine stateMachine;

        public StartDungeonState(GameStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void OnEnter() 
        {
            Debug.Log("Entering <color=red>StartDungeon</color> state");
        }
        public void OnUpdate() { }
        public void OnExit() { }
    }
    #endregion
}
