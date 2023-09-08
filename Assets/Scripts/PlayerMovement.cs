using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTransform;
    Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D feetCollider;
    private CapsuleCollider2D playerCollider;
    private float defaultGravityScale;
    public bool isAlive = true;
    private bool isJumping;
    private GameSession gameSession;
    public AudioSource audioSource;

    void Start()
    {
        InitializeFields();
    }


    void Update()
    {
        if (isAlive)
        {
            Run();
            Flip();
            Climb();
            Die();
        }
    }

    void InitializeFields()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        defaultGravityScale = rb.gravityScale;
        gameSession = GameSession.instance;
        isAlive = true;
        isJumping = false;
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    void Run()
    {
        rb.velocity = new Vector2(moveInput.x * movementSpeed, rb.velocity.y);

        if (!IsTouchingWall())
        {
            bool isMovingHorizontally = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
            animator.SetBool("isRunning", isMovingHorizontally);
        }
        else
            animator.SetBool("isRunning", false);
    }

    bool IsTouchingWall()
    {
        return playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }


    void Flip()
    {
        bool isMovingHorizontally = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (isMovingHorizontally)
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }

    bool IsTouchingGround()
    {
        return feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    bool CanJUmp()
    {
        return IsTouchingGround() || IsTouchingLadder();
    }

    void OnJump(InputValue inputValue)
    {
        if (isAlive)
        {
            if (inputValue.isPressed)
                Jump();
        }
    }

    void Jump()
    {
        if (isJumping)
        {
            animator.SetTrigger("Double Jump");
            rb.velocity += new Vector2(0f, jumpSpeed);
            isJumping = false;
        }
        else if (!isJumping && CanJUmp())
        {
            isJumping = true;
            rb.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    bool IsTouchingLadder()
    {
        return playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")) ||
         feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
    }

    void Climb()
    {
        if (IsTouchingLadder())
        {
            rb.velocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
            bool isMovingVertically = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
            animator.SetBool("isClimbing", isMovingVertically);
            rb.gravityScale = 0;
        }
        else if (!IsTouchingLadder() && rb.gravityScale == 0)
            rb.gravityScale = defaultGravityScale;
        else if (!IsTouchingLadder() && animator.GetBool("isClimbing"))
            animator.SetBool("isClimbing", false);
    }

    void Die()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) ||
        feetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards"))
        || playerCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            animator.SetTrigger("Death");
            isAlive = false;
            gameSession.processPlayerDeath();
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (isAlive)
        {
            if (inputValue.isPressed)
                Shoot();
        }

    }

    void Shoot()
    {
        animator.SetTrigger("Shoot");
        GameObject bullet = Instantiate(bulletPrefab, gunTransform.position, transform.rotation);
        bullet.transform.localScale = new Vector2(transform.localScale.x, 1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
            if (collision.relativeVelocity.y > 0)
                isJumping = false;
    }

    void OnExit()
    {
        Application.Quit();
    }

}
