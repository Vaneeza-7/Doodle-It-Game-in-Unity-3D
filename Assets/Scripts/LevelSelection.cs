using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public GameObject LevelSelectionpanel; // Reference to the panel GameObject

    public void LoadLevel(int level)
    { 
    SceneManager.LoadScene(level);
    }

    public void BackToHome()
    {
        LevelSelectionpanel.SetActive(false); // Deactivate the panel
        SceneManager.LoadScene("NewHome");
    }
}
