using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RhythmConfig 
{
    public Color tockColor;
    public float tickTime;
}

public class RhythmConfigurations : MonoBehaviour
{
    [SerializeField]
    private int tickAmount = 1;
    [SerializeField]
    private RhythmConfig[] rhythmConfigs;

    private void OnValidate()
    {
        if (tickAmount <= 0)
            tickAmount = 1;

        if (rhythmConfigs == null) 
        {
            rhythmConfigs = new RhythmConfig[1];
        }

        for (int i = 0; i < rhythmConfigs.Length; i++)
        {
            if (rhythmConfigs[i].tickTime <= 0)
                rhythmConfigs[i].tickTime = 0.5f;
        }
    }

    public RhythmConfig[] GetRhythmConfigsArray() 
    {
        return rhythmConfigs;
    }

    public RhythmConfig GetRhythmConfig(int tockIndex) 
    {
        return rhythmConfigs[tockIndex];
    }

    public int GetTockCount() 
    {
        return rhythmConfigs.Length;
    }

    public int GetTickAmount() 
    {
        return tickAmount;
    }
}
