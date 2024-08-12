using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BonusLevelManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public float levelDuration = 10f;

    private float timer;
    [SerializeField] public GameObject winPanel;
    private bool isBonusLevelEnded = false; // Add a flag to track if the bonus level has ended

    void Start()
    {
        timer = levelDuration;
        if (winPanel != null)
        {
            winPanel.SetActive(false);
            Debug.Log("Win Panel is set to inactive.");
        }
        else
        {
            Debug.LogError("Win Panel is not assigned!");
        }

        Time.timeScale = 1f; // Ensure the game is not paused at the start
    }

    void Update()
    {
        if (!isBonusLevelEnded) // Check if the bonus level has not ended
        {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Max(timer, 0).ToString("F2");
            scoreText.text = "Gem Count: " + ScoreManager.instance.score.ToString();

            if (timer <= 0)
            {
                EndBonusLevel();
            }
        }
    }

    void EndBonusLevel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        isBonusLevelEnded = true; // Set the flag to true
        Debug.Log("Bonus level ended! Final Score: " + ScoreManager.instance.score);
        // Additional logic can be added here if necessary
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Ensure the game is not paused in the next level
        // Assuming you are using SceneManager to load the next level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
