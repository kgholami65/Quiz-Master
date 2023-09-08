using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float loadDelay;
    [SerializeField] private int lastLevelIndex;
    private int currentSceneIndex;
    private GameSession gameSession;
    private AudioSource audioSource;
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameSession = GameSession.instance;
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if (currentSceneIndex == lastLevelIndex)
            {
                audioSource.Play();
                gameSession.DisableCanvas();
            }
            StartCoroutine(loadNextLevel());

        }
    }

    IEnumerator loadNextLevel()
    {
        yield return new WaitForSecondsRealtime(loadDelay);
        gameSession.totalScore += gameSession.thisSceneScore;
        gameSession.thisSceneScore = 0;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }


}
