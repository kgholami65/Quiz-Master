using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private float bounceSpeed;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && collision.collider.GetType() == typeof(BoxCollider2D))
        {
            audioSource.Play();
            collision.collider.attachedRigidbody.velocity = new Vector2(0f, bounceSpeed);
        }
    }
}
