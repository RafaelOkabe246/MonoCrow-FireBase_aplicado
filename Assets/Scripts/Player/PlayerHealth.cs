using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerAnimations playerAnim;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private SpriteRenderer playerSprite;

    [Space(20)]

    [Header("Attributes")]
    [SerializeField]
    private int hp = 1;

    public void TakeDamage() 
    {
        hp--;

        if (hp <= 0) 
        {
            PerformDeath();
        }
    }

    private void PerformDeath() 
    {
        playerController.enabled = false;
        playerAnim.SetTrigger("Death", false, true);

        TimeManager.instance.Sleep(0.1f);
        TimeManager.instance.CallSlowMotionWithDelay(0.5f, 0.5f, true);
        CameraController.instance.CameraShake(2f, 2f, 0.25f);

        //Death jump feedback
        playerSprite.transform.SetParent(null);
        Rigidbody2D spriteRig = playerSprite.gameObject.AddComponent<Rigidbody2D>();

        float chance = Random.Range(0f, 100f);
        Vector2 hForce = (chance <= 50) ? Vector2.right : Vector2.left;

        spriteRig.AddForce(800f * Vector2.up, ForceMode2D.Force);
        spriteRig.AddForce(300f * hForce);
        spriteRig.gravityScale = 2f;
    }

}
