using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0;
    private int originalScore = 0;
    public TMPro.TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
            LoadScore(); // Load the score when the instance is created
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        Debug.Log("Score: " + score);
        UpdateScoreText();
    }

    void Start()
    {
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        Debug.Log("Score: " + score);
        if (scoreText != null)
        {
            scoreText.text = "Gem Count: " + score.ToString();
        }
    }

    public void InitializeScoreText(TMPro.TextMeshProUGUI text)
    {
        scoreText = text;
        originalScore = score;
        UpdateScoreText();
    }

    public void ResetScore()
    {
       // score = 0;
        //SaveScore(); // Save the score when it's reset
        //UpdateScoreText();

        score = originalScore; // Reset score to the original value when entering the level
        UpdateScoreText();
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        SaveScore(); // Save the new score
        UpdateScoreText();
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save(); // Ensure the data is saved immediately
    }

    public void LoadScore()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");
        }
        else
        {
            score = 0;
        }
        UpdateScoreText();
    }

    public void DeleteScore()
    {
        PlayerPrefs.DeleteKey("Score");
        PlayerPrefs.Save();
    }

    public void AddHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save(); // Ensure the data is saved immediately
        }
    }
}
