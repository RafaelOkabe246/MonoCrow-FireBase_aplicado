using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmUI : MonoBehaviour
{
    [Header("Ticks")]
    [SerializeField]
    private GameObject tickUiPrefab;
    [SerializeField]
    private Transform ticksUiLayoutGroup;

    private void Start()
    {
        RhythmConfigurations rhythmConfig = RhythmManager.instance.GetRhythmConfig();
        int tickCount = rhythmConfig.GetTickAmount();

        for (int i = 0; i < tickCount; i++)
        {
            GameObject tickUiGO = Instantiate(tickUiPrefab, Vector3.zero, Quaternion.identity, ticksUiLayoutGroup);

            TickUI tickUI = tickUiGO.GetComponent<TickUI>();
            tickUI.SetIndex(i);
        }
    }
}
