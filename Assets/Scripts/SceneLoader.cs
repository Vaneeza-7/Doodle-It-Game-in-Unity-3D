using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
    public void LoadNextScene()
    {
        PlayClickSound();
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Load the next scene in the build index
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }

    public void ResetScene()
    {
        ScoreManager.instance.ResetScore();
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);
    }

    public void ExitGame()
    {

        Debug.Log("Game is exiting...");
        
        // Quit the application
        Application.Quit();
        
        // If running in the Unity editor, stop playing the scene
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
