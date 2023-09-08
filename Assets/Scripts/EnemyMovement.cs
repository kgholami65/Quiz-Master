using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Rigidbody2D rb;
    private CapsuleCollider2D colllider;
    private AudioSource audioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        colllider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Flip();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Bullet")
        {
            Destroy(collider.gameObject);
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")
        && collision.collider.GetType() == typeof(BoxCollider2D))
        {
            if (collision.collider.GetComponent<PlayerMovement>().isAlive)
                Die();
        }
    }


    void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, 1f);
        movementSpeed = -movementSpeed;
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
    }

    void Die()
    {
        audioSource.Play();
        rb.velocity = new Vector2(0, 12f);
        rb.isKinematic = false;
        colllider.enabled = false;
        gameObject.layer = LayerMask.GetMask("Default");
        Invoke(nameof(DestroySelf), 1f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
