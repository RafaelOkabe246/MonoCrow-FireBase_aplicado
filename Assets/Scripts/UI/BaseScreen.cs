using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    [SerializeField]
    private GameState enterState = GameState.Paused;
    [SerializeField]
    private GameState exitState = GameState.Gameplay;

    protected virtual void OnEnable()
    {
        GameManager.instance.SetState(enterState);
    }

    protected virtual void OnDisable()
    {
        GameManager.instance.SetState(exitState);
    }
}
