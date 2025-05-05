using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore = 0;
    public int winScore = 20;
    public Text scoreText;      // Assign in Inspector
    public GameObject winUI;    // Show when player wins

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
        if (winUI != null)
            winUI.SetActive(false);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();

        if (currentScore >= winScore)
        {
            WinGame();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
    }

    void WinGame()
    {
        SceneManager.LoadScene("SkibidiWinner");

        // Optional: Pause game
        Time.timeScale = 0f;
    }
}
