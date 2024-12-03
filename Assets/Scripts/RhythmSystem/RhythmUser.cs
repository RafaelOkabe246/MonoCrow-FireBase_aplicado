using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RhythmUser : MonoBehaviour
{
    [SerializeField]
    private UnityEvent TickFunctions;
    [SerializeField]
    private UnityEvent<Color> TockFunctions;

    private void OnEnable()
    {
        StartCoroutine(DelayToSetupDelegates());
    }

    private IEnumerator DelayToSetupDelegates() 
    {
        yield return new WaitForSeconds(0.05f);

        RhythmManager.instance.OnTick += OnTick;
        RhythmManager.instance.OnTock += OnTock;
    }

    private void OnDisable()
    {
        RhythmManager.instance.OnTick -= OnTick;
        RhythmManager.instance.OnTock -= OnTock;
    }

    private void OnTick() 
    {
        TickFunctions?.Invoke();
    }

    private void OnTock(Color newTockColor) 
    {
        TockFunctions?.Invoke(newTockColor);
    }
}
