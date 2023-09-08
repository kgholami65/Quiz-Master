using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSession : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    private GameSession gameSession;
    void Start()
    {
        gameSession = GameSession.instance;
        winText.text = "You won! Your score is: " + gameSession.totalScore;

    }

    public void RestartGame()
    {
        Destroy(gameSession.gameObject);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
