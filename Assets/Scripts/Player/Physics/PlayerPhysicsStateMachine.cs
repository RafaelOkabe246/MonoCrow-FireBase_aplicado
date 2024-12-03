using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the transitions between states of the player's physics
/// </summary>

public enum PlayerPhysicsStatesEnum
{
    NEUTRAL,
    JUMP
}

public class PlayerPhysicsStateMachine : MonoBehaviour
{
    private PlayerPhysicsState currentState;
    private PlayerPhysicsStatesEnum currentStateEnum;
    private Dictionary<PlayerPhysicsStatesEnum, PlayerPhysicsState> physicsStatesDict;

    #region COMPONENTS
    [SerializeField]
    private PlayerPhysics playerPhysics;
    #endregion

    private void Awake()
    {
        BuildStates();
        ForcedChangeState(PlayerPhysicsStatesEnum.NEUTRAL);
    }

    private void Update()
    {
        currentState.ExecuteState();
    }

    private void BuildStates()
    {
        physicsStatesDict = new Dictionary<PlayerPhysicsStatesEnum, PlayerPhysicsState>();

        physicsStatesDict.Add(PlayerPhysicsStatesEnum.NEUTRAL, new PlayerNeutralState(playerPhysics, this));
        physicsStatesDict.Add(PlayerPhysicsStatesEnum.JUMP, new PlayerJumpState(playerPhysics, this));
    }

    public void ChangeState(PlayerPhysicsStatesEnum newState)
    {
        if (currentStateEnum == newState)
            return;

        if (currentState != null)
            currentState.ExitState(newState);

        PlayerPhysicsStatesEnum previousState = currentStateEnum;

        currentState = physicsStatesDict[newState];
        currentStateEnum = newState;
        currentState.EnterState(previousState);
    }

    private void ForcedChangeState(PlayerPhysicsStatesEnum newState)
    {
        if (currentState != null)
            currentState.ExitState(newState);

        PlayerPhysicsStatesEnum previousState = currentStateEnum;

        currentState = physicsStatesDict[newState];
        currentStateEnum = newState;
        currentState.EnterState(previousState);
    }

    public void QuitState()
    {
        ChangeState(currentState.PreviousStateEnum);
    }

    public PlayerPhysicsStatesEnum GetCurrentState()
    {
        return currentStateEnum;
    }
}

