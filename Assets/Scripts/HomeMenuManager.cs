using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject levelSelectionPanel; // Reference to the LevelSelection panel
   // public GameObject settingsPanel; // Reference to the Settings panel
    public GameObject volumePanel; // Reference to the Volume panel

    public void OpenLevelSelectionPanel()
    {
        levelSelectionPanel.SetActive(true);
      //  settingsPanel.SetActive(false); // Ensure settings panel is closed
        volumePanel.SetActive(false); // Ensure volume panel is closed
    }

    public void OpenSettingsPanel()
    {
      //  settingsPanel.SetActive(true);
        levelSelectionPanel.SetActive(false); // Ensure level selection panel is closed
        volumePanel.SetActive(true); // Ensure volume panel is opened
    }

    public void CloseAllPanels()
    {
        levelSelectionPanel.SetActive(false);
      //  settingsPanel.SetActive(false);
        volumePanel.SetActive(false);
    }

    public void VolumeOn()
    {
        AudioListener.volume = 1f; // Set volume to full
        Debug.Log("Volume On");
    }

    public void VolumeOff()
    {
        AudioListener.volume = 0f; // Mute volume
        Debug.Log("Volume Off");
    }

    public void BackToHomePage()
    {
        // Assuming you have a Home scene named "HomeScene"
        UnityEngine.SceneManagement.SceneManager.LoadScene("NewHome");
        Debug.Log("Back to Home Page");
    }
}
