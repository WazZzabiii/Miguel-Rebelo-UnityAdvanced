using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float groundCheckOffset;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float attackCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private HealthBar healthBar;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + Vector3.right * groundCheckOffset, Vector2.down * groundCheckDistance, Color.red);
        Debug.DrawRay(transform.position + Vector3.left * groundCheckOffset, Vector2.down * groundCheckDistance, Color.red);

        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * movementSpeed * Time.deltaTime);
        GroundCheck();
    }

    private void GroundCheck()
    {
        Vector3 rayOriginRight = transform.position + Vector3.right * groundCheckOffset;
        Vector3 rayOriginLeft = transform.position + Vector3.left * groundCheckOffset;

        RaycastHit2D groundHitRight = Physics2D.Raycast(rayOriginRight, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D groundHitLeft = Physics2D.Raycast(rayOriginLeft, Vector2.down, groundCheckDistance, groundLayer);

        if ((facingRight && !groundHitRight) || (!facingRight && !groundHitLeft))
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player playerDetected = collision.gameObject.GetComponent<Player>();
        if (playerDetected != null)
        {
            healthBar.TakeDamage((int)damage);
        }
    }
}


