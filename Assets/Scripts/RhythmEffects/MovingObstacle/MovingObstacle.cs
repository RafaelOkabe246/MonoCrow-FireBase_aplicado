using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : RhythmEffect
{
    [SerializeField]
    private Rigidbody2D rig;
    [SerializeField]
    private Vector2 moveSpeed;
    [SerializeField]
    private float xDistance;
    [SerializeField]
    private float yDistance;
    private Vector2 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        ToggleMovement();
    }

    public override void DisableOnTock()
    {
        base.DisableOnTock();

        ToggleMovement();
    }

    private void ToggleMovement()
    {
        rig.velocity = (isEnabled) ? new Vector2(moveSpeed.x, moveSpeed.y) : Vector2.zero;
    }

    private void Update()
    {
        if (!isEnabled)
            return;

        if (Mathf.Abs(transform.position.x) > initialPosition.x + xDistance/2 || 
            transform.position.x < initialPosition.x - xDistance / 2)
            InvertXmovement();
        if (transform.position.y > initialPosition.y + yDistance / 2 || 
            transform.position.y < initialPosition.y - yDistance / 2)
            InvertYMovement();
    }

    private void InvertXmovement()
    {
        rig.velocity = new Vector2(-rig.velocity.x, rig.velocity.y);
        transform.position = new Vector2((initialPosition.x + xDistance/2) * -(rig.velocity.x / Mathf.Abs(rig.velocity.x)), transform.position.y);
    }

    private void InvertYMovement() 
    {
        rig.velocity = new Vector2(rig.velocity.x, -rig.velocity.y);
        transform.position = new Vector2(transform.position.x, (initialPosition.y) + (yDistance / 2 * -(rig.velocity.y / Mathf.Abs(rig.velocity.y))));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x - xDistance, transform.position.y, transform.position.z), new Vector3(transform.position.x + xDistance, transform.position.y, transform.position.z));
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - yDistance, 0f), new Vector3(transform.position.x, transform.position.y + yDistance, transform.position.z));
    }
}
