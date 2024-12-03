using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RhythmManager : Singleton<RhythmManager>
{
    public delegate void Tock(Color newTockColor);
    public Tock OnTock;
    public delegate void Tick();
    public Tick OnTick;

    private RhythmConfigurations rhythmConfig;

    private float tickTimeCount;
    public int CurrentTick { get; private set; }
    public int CurrentTock { get; private set; }

    private bool isPaused;

    #region INITIALIZATION
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        rhythmConfig = FindObjectOfType<RhythmConfigurations>();
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        isPaused = newGameState == GameState.Paused;
    }

    private void Start()
    {
        StartCoroutine(CallFirstTock());
    }

    private IEnumerator CallFirstTock() 
    {
        yield return new WaitForSeconds(0.1f);

        OnTock?.Invoke(rhythmConfig.GetRhythmConfig(CurrentTock).tockColor);
    }
    #endregion

    private void Update()
    {
        if (isPaused)
            return;

        TickTime();
    }

    private void TickTime()
    {
        tickTimeCount += Time.deltaTime;

        if (tickTimeCount >= rhythmConfig.GetRhythmConfig(CurrentTock).tickTime)
        {
            tickTimeCount = 0;
            CurrentTick++;

            TockTime();

            OnTick?.Invoke();
        }
    }

    private void TockTime()
    {
        if (CurrentTick > rhythmConfig.GetTickAmount())
        {
            CurrentTick = 0;
            CurrentTock++;

            if (CurrentTock >= rhythmConfig.GetTockCount())
                CurrentTock = 0;

            OnTock?.Invoke(rhythmConfig.GetRhythmConfig(CurrentTock).tockColor);
        }
    }

    #region GETTERS
    public RhythmConfigurations GetRhythmConfig() 
    {
        return rhythmConfig;
    }
    
    #endregion
}
