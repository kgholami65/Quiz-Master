using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerLives;
    [SerializeField] private float deathDelay;
    [SerializeField] private TextMeshProUGUI livesCountText;
    [SerializeField] private TextMeshProUGUI scoreCountText;
    private Canvas canvas;
    public static GameSession instance;
    public int thisSceneScore;
    public int totalScore;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int loseMenuIndex;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    void Start()
    {
        livesCountText.text = "" + playerLives;
        scoreCountText.text = "" + totalScore;
        canvas = GetComponentInChildren<Canvas>();
    }

    public void processPlayerDeath()
    {
        if (playerLives > 1)
            Invoke(nameof(TakeLife), deathDelay);
        else
        {
            audioSource.Play();
            Invoke(nameof(ResetGame), deathDelay);

        }
    }

    void ResetGame()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(loseMenuIndex);
    }

    public void AddToScore(int pointsToAdd)
    {
        thisSceneScore += pointsToAdd;
        scoreCountText.text = (totalScore + thisSceneScore).ToString();
    }

    void TakeLife()
    {
        playerLives--;
        thisSceneScore = 0;
        livesCountText.text = "" + playerLives;
        scoreCountText.text = "" + totalScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DisableCanvas()
    {
        canvas.gameObject.SetActive(false);
    }

}
