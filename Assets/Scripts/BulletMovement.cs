using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetMovement();
    }

    void SetMovement()
    {
        rb.velocity = new Vector2(movementSpeed * transform.localScale.x, 0f);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
            Destroy(gameObject);
    }
}
