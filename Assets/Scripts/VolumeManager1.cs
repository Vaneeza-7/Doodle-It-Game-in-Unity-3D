using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolumeSelection : MonoBehaviour
{
    public GameObject VolumeSelectionpanel; // Reference to the panel GameObject
    public AudioSource backgroundAudio; // Reference to the AudioSource component
    public GameObject VolumeON; // Reference to the VolumeON button
    public GameObject VolumeOff; // Reference to the VolumeOff button

    private bool isSoundOn = true; // To track the sound state

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void BackToHome()
    {
        VolumeSelectionpanel.SetActive(false); // Deactivate the panel
        SceneManager.LoadScene("NewHome");
    }

    public void TurnSoundOn()
    {
        isSoundOn = true;
        backgroundAudio.Play();
        VolumeON.SetActive(false); // Hide the VolumeON button
        VolumeOff.SetActive(true); // Show the VolumeOff button
    }

    public void TurnSoundOff()
    {
        isSoundOn = false;
        backgroundAudio.Pause();
        VolumeON.SetActive(true); // Show the VolumeON button
        VolumeOff.SetActive(false); // Hide the VolumeOff button
    }
}
