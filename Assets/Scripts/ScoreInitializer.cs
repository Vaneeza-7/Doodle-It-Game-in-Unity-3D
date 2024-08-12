using UnityEngine;
using TMPro;

public class ScoreInitializer : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.InitializeScoreText(scoreText);
        }
    }
}
