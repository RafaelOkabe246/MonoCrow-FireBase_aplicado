using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmEffect : MonoBehaviour
{
    protected bool isEnabled = true;
    [SerializeField]
    protected List<int> indexesToDisable;

    public virtual void DisableOnTock() 
    {
        if (indexesToDisable.Count == 0)
            return;

        int currentTock = RhythmManager.instance.CurrentTock;

        isEnabled = (indexesToDisable.Contains(currentTock)) ? true : false;
    }
}
