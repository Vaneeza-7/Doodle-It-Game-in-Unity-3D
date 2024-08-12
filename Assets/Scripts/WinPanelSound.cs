using UnityEngine;

public class WinPanelSound : MonoBehaviour
{
    public GameObject winPanel; // Reference to the win panel
    public AudioClip goodJobSound; // Assign the good job sound clip in the inspector
    public ParticleSystem uiConfettiParticleSystem; // Reference to the UI confetti particle system

    private AudioSource audioSource;
    private bool soundPlayed = false; // Track if the sound has been played

    public int LevelNumber; // Level number to be set in the inspector

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = goodJobSound;
        audioSource.loop = false;

        if (winPanel == null)
        {
            Debug.LogError("Win Panel is not assigned!");
        }

        if (uiConfettiParticleSystem == null)
        {
            Debug.LogError("UI Confetti Particle System is not assigned!");
        }
        else
        {
            // Ensure the particle system is initially stopped
            var main = uiConfettiParticleSystem.main;
            main.playOnAwake = false;
            main.useUnscaledTime = true; // Use unscaled time
            uiConfettiParticleSystem.Stop();
        }
    }

    void Update()
    {
        if (winPanel == null || uiConfettiParticleSystem == null) return;

        if (winPanel.activeSelf && !soundPlayed)
        {
            PlayGoodJobSound();
            PlayUIConfetti();
            soundPlayed = true;
        }
        else if (!winPanel.activeSelf)
        {
            soundPlayed = false;
        }
    }

    private void PlayGoodJobSound()
    {
        if (audioSource != null && goodJobSound != null)
        {
            audioSource.Play();
        }
    }

    private void PlayUIConfetti()
    {
        if (uiConfettiParticleSystem != null)
        {
            uiConfettiParticleSystem.transform.SetParent(winPanel.transform, false);
            //a bit high on y axis
            if(LevelNumber == 10 || LevelNumber == 3 || LevelNumber == 4)
            {
            uiConfettiParticleSystem.transform.localPosition = new Vector3(0, 3f, 0);
            }
            else
            {
            uiConfettiParticleSystem.transform.localPosition = Vector3.zero; // Adjust as needed
            }
            uiConfettiParticleSystem.Play();
        }
    }
}
