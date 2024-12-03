using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rig;

    public void SetupBullet(Vector2 dir, float speed) 
    {
        rig.velocity = dir * speed;
    }
}
