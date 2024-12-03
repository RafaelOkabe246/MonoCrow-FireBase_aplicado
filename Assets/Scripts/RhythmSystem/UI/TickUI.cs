using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickUI : MonoBehaviour
{
    private int index;
    [SerializeField]
    private GameObject tickFill;

    public void SetIndex(int newIndex) 
    {
        index = newIndex;
    }

    public void OnTick() 
    {
        bool active = (RhythmManager.instance.CurrentTick > index) ? true : false;

        tickFill.SetActive(active);
    }
}
