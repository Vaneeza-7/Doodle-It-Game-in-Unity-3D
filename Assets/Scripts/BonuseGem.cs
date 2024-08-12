using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGem : MonoBehaviour
{
    public AudioClip collectSound; // Assign the collect sound clip in the Inspector

    private AudioSource audioSource;

    public ParticleSystem glitterParticles;

    public GemCollectionEffect gemCollectionEffect;
    public Material gemMaterial; // Assign the gem's material in the Inspector

    private void Start()
    {
        // Get the existing AudioSource component
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the GameObject.");
        }
        if (glitterParticles == null)
        {
            Debug.LogError("Glitter Particles are not assigned!");
        }

        if (gemMaterial == null)
        {
            Debug.LogError("Gem material is not assigned!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with the gem");

            // Play collect sound
            PlayCollectSound();
            // Play glitter particles
            PlayGlitterParticles();

            // Show the +2 effect
            if(gemCollectionEffect != null)
            {
                gemCollectionEffect.ShowGemCollected();
            }

            // Assign the gem's material color to the player's line renderer
            LineRenderer playerLineRenderer = other.gameObject.GetComponent<LineRenderer>();
            if (playerLineRenderer != null)
            {
                playerLineRenderer.material = gemMaterial;
                Debug.Log("Line Renderer color changed to the gem's color");
            }
            else
            {
                Debug.LogError("Player does not have a LineRenderer component.");
            }

            Destroy(gameObject, collectSound.length);
            Debug.Log("Gem Collected");
            ScoreManager.instance.AddScore(2);
        }

        if (other.gameObject.CompareTag("Sea"))
        {
            Debug.Log("Gem collided with the sea and will be destroyed");
            Destroy(gameObject);
        }
    }

    private void PlayCollectSound()
    {
        if (collectSound != null && audioSource != null)
        {
            audioSource.clip = collectSound;
            audioSource.volume = 1.0f; // Ensure volume is set to a reasonable level
            audioSource.mute = false;  // Ensure the AudioSource is not muted
            audioSource.Play();
            Debug.Log("Playing collect sound: " + collectSound.name);

            // Optionally wait for the sound to finish before destroying the object
            StartCoroutine(DestroyAfterSound());
        }
        else
        {
            Debug.LogError("collectSound AudioClip is not assigned in the Inspector or AudioSource component is missing.");
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        // Wait for the clip to finish playing
        yield return new WaitForSeconds(collectSound.length);
        Destroy(gameObject);
    }

    private void PlayGlitterParticles()
    {
        if (glitterParticles != null)
        {
            glitterParticles.Play();
        }
    }
}
