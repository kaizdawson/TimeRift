using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding1 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback Knockback;

    private void Awake()
    {
        Knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Knockback.GettingKnockedBack) { return; }

        if (moveDir != Vector2.zero) // Chỉ di chuyển nếu có hướng
        {
            rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
        }
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = (targetPosition - (Vector2)transform.position).normalized; // Lấy hướng di chuyển
    }
    public void StopMoving()
    {
        moveDir = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

}
