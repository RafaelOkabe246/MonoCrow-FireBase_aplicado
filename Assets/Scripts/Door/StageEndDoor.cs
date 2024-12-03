using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageEndDoor : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnStageEnded;

    public void EndStage() 
    {
        StartCoroutine(EndStageCoroutine());
    }

    private IEnumerator EndStageCoroutine() 
    {
        yield return new WaitForSeconds(0.5f);

        OnStageEnded?.Invoke();
    }
}
