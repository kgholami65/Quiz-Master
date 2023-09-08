using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int score;
    private bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            collider.gameObject.GetComponent<PlayerMovement>().audioSource.Play();
            GameSession.instance.AddToScore(score);
            Destroy(gameObject);
        }
    }

}
