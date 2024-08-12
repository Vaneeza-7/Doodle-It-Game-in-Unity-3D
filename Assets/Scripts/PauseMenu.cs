using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public GameObject PauseMenuPanel;
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;

    public static bool GameIsPaused = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void Pause()
    {
        PlayClickSound();
        PauseMenuPanel.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        PlayClickSound();
        PauseMenuPanel.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        PlayClickSound();
        ScoreManager.instance.ResetScore();
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("LEVEL 2 GREEN");

    }

    public void LoadNextScene()
    {
        PlayClickSound();
        // Reset time scale before loading the next scene
        Time.timeScale = 1f;
        GameIsPaused = false;
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Load the next scene in the build index
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }

    public void ResetScene()
    {
        PlayClickSound();
        ScoreManager.instance.ResetScore();
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Home()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("NewHome");
        GameIsPaused = false;
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
