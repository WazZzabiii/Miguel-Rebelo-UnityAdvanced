using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking;
    private bool isGrounded;
    private bool isWalking;
    private bool forceUnground;
    Vector2 startPos;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce = 2;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float enemyCheckDistance;
    [SerializeField] GameManager gameManager;
    [SerializeField] private Slider oxygenBar;
    [SerializeField] private float maxOxygen;
    [SerializeField] private float currentOxygen;




    private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            startPos = transform.position;
            HealthBar healthBar = GetComponent<HealthBar>();

        }

        void Update()
        { if(currentOxygen <= 0)
        {
            return;
        }
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * enemyCheckDistance, Color.magenta, 1);
            //Movement
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (isAttacking)
            {
                horizontalInput = 0;
            }

            if (horizontalInput != 0)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }


            if (horizontalInput > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontalInput < 0)
            {
                spriteRenderer.flipX = true;
            }
            Jump();
            GroundCheck();
            rigidBody.velocity = new Vector2(horizontalInput * movementSpeed, rigidBody.velocity.y);

            currentOxygen -= Time.deltaTime;
            oxygenBar.value = currentOxygen;

             if (currentOxygen <= 0)
            {
            Time.timeScale = 0f;
            gameManager.GameOver();
            }
    }

        

        private void GroundCheck()
        {
            Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);


            if (hit && forceUnground == false)
            {
                isGrounded = true;
                print("you are grounded");
            }
            else
            {
                isGrounded = false;
            }

        }
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
            {
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                StartCoroutine(ForceUngroundTimer());
                print("Jump");
            }
        }

        private IEnumerator ForceUngroundTimer()
        {
            forceUnground = true;
            yield return new WaitForSeconds(0.5f);
            forceUnground = false;
        }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Player Collided");
            Fall();
        }
    }

    public void Fall()
    {
        StartCoroutine(Respawn(0.5f));
        
    }

    IEnumerator Respawn(float duration)
    {
        yield return new WaitForSeconds(duration);
        transform.position = startPos;
        HealthBar.health = 50;
    }
}