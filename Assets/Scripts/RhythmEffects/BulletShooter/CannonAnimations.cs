using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void TriggerShootingAnimation() 
    {
        anim.SetTrigger("Shoot");
    }
}
