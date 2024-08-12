using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelController : MonoBehaviour
{
    public GameObject tutorialPanel;
    private static bool tutorialShown = false;

    void Start()
    {
        // Show the tutorial panel at the start of the level if it hasn't been shown yet
        if (!tutorialShown)
        {
            ShowTutorialPanel();
            tutorialShown = true; // Mark the tutorial as shown
        }
        else
        {
            CloseTutorialPanel(); // Ensure the panel is closed if it was shown previously
        }
    }

    void ShowTutorialPanel()
    {
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorialPanel()
    {
        tutorialPanel.SetActive(false);
    }
}
