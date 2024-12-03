using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGeneralAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void TriggerTockAnimation(Color newColor) 
    {
        anim.SetTrigger("Tock");
    }

    public void TriggerTickAnimation()
    {
        anim.SetTrigger("Tick");
    }
}
