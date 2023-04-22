using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    public float moveSpeed = 40.0f;
    public Rigidbody2D rb;
    public Animator animator;
    private AnimatorStateInfo stateInfo;
    private bool isTouchingWall = false;
    private Vector2 wallNormal = Vector2.zero;
    private int wallContactCount = 0;

    Vector2 movement;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GlobalVariables.deadLabel == 1)
        {
            animator.SetBool("die", true);
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1 && !animator.IsInTransition(0))
            {
                Time.timeScale = 0;
            }
        }
        if (Time.timeScale == 0)
        {
            return;
        }

        MovePlayer(); // move player using H & V axis (WASD)

        if (isTouchingWall)
        {
            // Project the input onto the wall normal
            float dotProduct = Vector2.Dot(movement, wallNormal);

            // If the input is in the direction of the wall, zero out the input along the wall normal
            if (dotProduct > 0)
            {
                Vector2 projectedVector = dotProduct * wallNormal;
                movement -= projectedVector;
            }
        }

        animator.SetFloat("H", movement.x);
        animator.SetFloat("V", movement.y);
        if (movement.x > 0.01)
        {
            spriteRenderer.flipX = true;
        }
        if (movement.x < 0.01)
        {
            spriteRenderer.flipX = false;
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void MovePlayer()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Wall"))
    {
        wallContactCount++;
        wallNormal = collision.contacts[0].normal;
    }
}

void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Wall"))
    {
        wallContactCount--;
        if (wallContactCount <= 0)
        {
            wallContactCount = 0;
            wallNormal = Vector2.zero;
        }
    }
}

}
