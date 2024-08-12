using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGem : MonoBehaviour
{
    public float speed = 10f;
    public Transform player;

    public AudioClip collectSound; // Add this field to assign the collect sound clip
    public AudioClip goodJobSound; // Add this field to assign the good job sound clip
    [SerializeField] public GameObject winPanel;

    // Reference to the particle system
    public ParticleSystem glitterParticles;

    // Set the win animation on the player
    Animator playerAnimator;

    private bool isCollected = false; // Add a flag to prevent multiple collections

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerAnimator = player.GetComponent<Animator>();
            playerAnimator.SetBool("win", false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
            Debug.Log("Win Panel is set to inactive.");
        }
        else
        {
            Debug.LogError("Win Panel is not assigned!");
        }

        if (glitterParticles == null)
        {
            Debug.LogError("Glitter Particles are not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.gameObject.tag == "Player" && !isCollected)
        {
            isCollected = true;
            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null && !playerAnimator.GetBool("isDead"))
            {
                // Play collect sound
                PlayCollectSound();

                // Play glitter particles
                PlayGlitterParticles();

                // Set the win animation on the player
                playerAnimator.SetBool("win", true);

                // Start coroutine to handle gem destruction and win panel activation
                StartCoroutine(HandleGemCollected());
                Debug.Log("Gem Collected");
                ScoreManager.instance.AddScore(1);
            }
        }
    }

    private void Update()
    {
        if (player == null || isCollected) return;

        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) < 0.01f)
            {
                if (playerAnimator != null && !playerAnimator.GetBool("isDead"))
                {
                    // Ensure the gem is only collected once
                    isCollected = true;

                    // Play collect sound
                    PlayCollectSound();

                    // Play glitter particles
                    PlayGlitterParticles();

                    // Set the win animation on the player
                    playerAnimator.SetBool("win", true);

                    // Start coroutine to handle gem destruction and win panel activation
                    StartCoroutine(HandleGemCollected());
                    Debug.Log("Gem Collected");
                    ScoreManager.instance.AddScore(1);
                }
            }
        }
    }

    private void PlayCollectSound()
    {
        // Create an AudioSource component dynamically
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = collectSound;
        audioSource.Play();

        // Remove the AudioSource component after the sound has played
        Destroy(audioSource, collectSound.length);
    }

    private void PlayGlitterParticles()
    {
        if (glitterParticles != null)
        {
            glitterParticles.Play();
        }
    }

    private IEnumerator HandleGemCollected()
    {
        // Wait for the collect sound to finish playing
        yield return new WaitForSeconds(collectSound.length);

        // Destroy the gem
        Destroy(gameObject);

        // Activate the win panel and pause the game
        setWinPanelActive();
    }

    private void PlayGoodJobSound()
    {
        // Create an AudioSource component dynamically
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = goodJobSound;
        audioSource.Play();

        // Remove the AudioSource component after the sound has played
        Destroy(audioSource, goodJobSound.length);
    }

    private void setWinPanelActive()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            // Play the good job sound
            //PlayGoodJobSound();
            Time.timeScale = 0f;
        }
    }
}
