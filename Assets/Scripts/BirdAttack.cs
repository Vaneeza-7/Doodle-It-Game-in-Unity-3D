using System.Collections;
using UnityEngine;

public class BirdAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    public GameObject[] birds; // Array of bird GameObjects
    private Animator[] birdAnimators;
    public float speed = 10f;
    public Transform[] birdTargets; // Array of bird Transforms

    public float deathDelay = 1f; // Time to wait before destroying the game object
    public float gameOverDelay = 2f; // Time to wait before showing the GameOver panel

    private bool isDead = false; // To track if the player is dead

    public GameObject gameOverPanel; // Reference to the GameOver panel
    public AudioClip deathSound; // Add this field to assign the death sound clip
    private AudioSource audioSource; // Add an AudioSource component

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        if (birds == null || birds.Length == 0)
        {
            Debug.LogError("Birds array is not assigned or empty!");
            return;
        }

        birdAnimators = new Animator[birds.Length];
        for (int i = 0; i < birds.Length; i++)
        {
            if (birds[i] != null)
            {
                birdAnimators[i] = birds[i].GetComponent<Animator>();
                if (birdAnimators[i] == null)
                {
                    Debug.LogError("No Animator component found on bird: " + birds[i].name);
                }
                else
                {
                    birdAnimators[i].SetBool("hit", false);
                }
            }
            else
            {
                Debug.LogError("Bird at index " + i + " is null!");
            }
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Ensure the GameOver panel is initially inactive
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool win = animator.GetBool("win");

        if (!isDead && !win && (collision.gameObject.tag == "Sea" || collision.gameObject.tag == "Sparrow"))
        {
            isDead = true;
            animator.SetBool("isDead", true); // Set the isDead parameter to true
            PlayDeathSound(); // Play the death sound
            for (int i = 0; i < birds.Length; i++)
            {
                if (collision.gameObject == birds[i])
                {
                    birdAnimators[i].SetBool("hit", true);
                }
            }
            rb.velocity = Vector3.zero; // Stop any current movement
            rb.isKinematic = true; // Make the Rigidbody kinematic to prevent further physics interactions
            StartCoroutine(HandleDeath()); // Start coroutine to handle death and show the game over panel
        }
    }

    private void Update()
    {
        bool win = animator.GetBool("win");
        if (!isDead && !win && birdTargets != null && birdTargets.Length > 0)
        {
            for (int i = 0; i < birdTargets.Length; i++)
            {
                if (Vector3.Distance(transform.position, birdTargets[i].position) < 2f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, birdTargets[i].position, speed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, birdTargets[i].position) < 0.01f)
                    {
                        isDead = true;
                        animator.SetBool("isDead", true); // Set the isDead parameter to true
                        PlayDeathSound(); // Play the death sound
                        birdAnimators[i].SetBool("hit", true);
                        rb.velocity = Vector3.zero; // Stop any current movement
                        rb.isKinematic = true; // Make the Rigidbody kinematic to prevent further physics interactions
                        StartCoroutine(HandleDeath()); // Start coroutine to handle death and show the game over panel
                        break;
                    }
                }
            }
        }
    }

    private void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(deathDelay); // Wait for the specified death delay
        ShowGameOverPanel(); // Show the GameOver panel
        yield return new WaitForSeconds(gameOverDelay); // Wait for the specified game over delay
        Destroy(gameObject); // Destroy the game object after the delay
    }

    private void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Debug.LogError("GameOver panel is not assigned!");
        }
    }
}
