using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The states are responsible for handling the logic of each state of the player's physics. Acesses the variables necessary for its funcionalities from the PlayerPhysics class
/// </summary>

public abstract class PlayerPhysicsState
{
    protected PlayerPhysics playerPhysics;
    protected PlayerPhysicsStateMachine playerPhysicsStateMachine;
    public PlayerPhysicsStatesEnum PreviousStateEnum = PlayerPhysicsStatesEnum.NEUTRAL;

    public PlayerPhysicsState(PlayerPhysics _playerPhysics, PlayerPhysicsStateMachine _playerSM)
    {
        playerPhysics = _playerPhysics;
        playerPhysicsStateMachine = _playerSM;
    }

    public virtual void EnterState(PlayerPhysicsStatesEnum previousState)
    {
        PreviousStateEnum = previousState;
    }
    public virtual void ExecuteState() { }

    public virtual void ExitState(PlayerPhysicsStatesEnum nextState) { }
}

#region GROUNDED
public class PlayerNeutralState : PlayerPhysicsState
{
    public PlayerNeutralState(PlayerPhysics _playerPhysics, PlayerPhysicsStateMachine _playerSM) : base(_playerPhysics, _playerSM)
    {
        playerPhysics = _playerPhysics;
        playerPhysicsStateMachine = _playerSM;
    }

    public override void EnterState(PlayerPhysicsStatesEnum previousState)
    {
        base.EnterState(previousState);
    }

    public override void ExecuteState()
    {
        base.ExecuteState();

        playerPhysics.SetGravityScale(playerPhysics.PhysicsData.gravityScale);
    }

    public override void ExitState(PlayerPhysicsStatesEnum nextState)
    {
        base.ExitState(nextState);
    }
}
#endregion

#region JUMP
public class PlayerJumpState : PlayerPhysicsState
{
    public PlayerJumpState(PlayerPhysics _playerPhysics, PlayerPhysicsStateMachine _playerSM) : base(_playerPhysics, _playerSM)
    {
        playerPhysics = _playerPhysics;
        playerPhysicsStateMachine = _playerSM;
    }

    public override void EnterState(PlayerPhysicsStatesEnum previousState)
    {
        base.EnterState(previousState);

        playerPhysics.PlayerAnim.SetBool("isJumping", true, false, true);
        playerPhysics.PlayerAnim.CallJumpFeedback();
    }

    public override void ExecuteState()
    {
        base.ExecuteState();

        if (playerPhysics.Rig.velocity.y < 0)
        {
            playerPhysics.IsJumpFalling = true;
            playerPhysics.PlayerAnim.SetBool("isJumping", false, false, true);

            playerPhysicsStateMachine.QuitState();
            return;
        }

        if (playerPhysics.IsJumpFalling && Mathf.Abs(playerPhysics.Rig.velocity.y) < playerPhysics.PhysicsData.jumpHangTimeThreshold)
            playerPhysics.SetGravityScale(playerPhysics.PhysicsData.gravityScale * playerPhysics.PhysicsData.jumpHangGravityMult);
    }

    public override void ExitState(PlayerPhysicsStatesEnum nextState)
    {
        base.ExitState(nextState);

        playerPhysics.PlayerAnim.SetBool("isJumping", false, false, true);
    }
}
#endregion
