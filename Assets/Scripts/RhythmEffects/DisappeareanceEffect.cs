using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappeareanceEffect : RhythmEffect
{
    [Space(15)]

    [Header("Disappeareance Effect")]
    [SerializeField]
    private List<int> indexesToDisappear;
    [SerializeField]
    private GameObject[] objectsToDeactivate;

    public void DisappearOnTock(Color newColor) 
    {
        if (!isEnabled)
            return;

        Disappear();
    }

    public void Disappear()
    {
        if (indexesToDisappear.Count == 0)
            return;

        int currentTock = RhythmManager.instance.CurrentTock;

        if (indexesToDisappear.Contains(currentTock))
            ToggleGameObjects(false);
        else
            ToggleGameObjects(true);
    }

    private void ToggleGameObjects(bool result) 
    {
        foreach (GameObject go in objectsToDeactivate)
        {
            go.SetActive(result);
        }
    }
}
