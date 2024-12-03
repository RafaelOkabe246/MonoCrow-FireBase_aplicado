using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{

    [Header("References")]
    [SerializeField]
    private Collider2D col;
    [SerializeField]
    private PlayerHealth playerHealth;

    private const int enemyLayer = 8;
    private const int doorLayer = 9;

    private void OnEnable()
    {
        col.enabled = true;
    }

    private void OnDisable()
    {
        col.enabled = false;
    }

    private void TakeDamage()
    {
        playerHealth.TakeDamage();
        this.enabled = false;
    }

    private void PerformVictory() 
    {
        PlayerAnimations playerAnim = gameObject.GetComponent<PlayerAnimations>();
        playerAnim.SetTrigger("Victory", false, true);
        //Call victory screen
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == enemyLayer) 
        {
            TakeDamage();
        }
        else if(collision.gameObject.layer == doorLayer)
        {
            PerformVictory();
            collision.gameObject.GetComponent<StageEndDoor>().EndStage();
        }
    }
}
