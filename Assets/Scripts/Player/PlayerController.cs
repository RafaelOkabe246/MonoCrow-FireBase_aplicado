using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region REFERENCES
    [Header("References")]

    [SerializeField]
    private PlayerPhysicsStateMachine physicsStateMachine;
    [SerializeField]
    private PlayerPhysics playerPhysics;
    #endregion

    private bool isPaused = false;

    private void OnEnable()
    {
        StartCoroutine(DelayToSetupDelegates());
    }

    private void OnDisable()
    {
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private IEnumerator DelayToSetupDelegates() 
    {
        yield return new WaitForSeconds(0.1f);

        GameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState) 
    {
        isPaused = newGameState == GameState.Paused;
    }

    public void MovePlayer(InputAction.CallbackContext value)
    {
        if (isPaused)
            return;

        Vector2 input = value.ReadValue<Vector2>();

        if (input.x > 0 && input.x < 1f)
            input = new Vector2(1f, input.y);
        else if (input.x < 0 && input.x > -1f)
            input = new Vector2(-1f, input.y);

        playerPhysics.OnMoveInput(input);
    }

    public void Jump(InputAction.CallbackContext value)
    {
        if (isPaused)
            return;

        if (value.canceled)
        {
            playerPhysics.OnJumpUpInput();
            return;
        }

        playerPhysics.OnJumpInput();
    }
}
