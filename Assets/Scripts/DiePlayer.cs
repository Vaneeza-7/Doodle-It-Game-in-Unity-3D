using System.Collections;
using UnityEngine;

public class DiePlayer : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    public float speed = 10f;
    public Transform shark;
    public float deathDelay = 3f; // Time to wait before destroying the game object
    public float gameOverDelay = 4f; // Time to wait before showing the GameOver panel
    private bool isDead = false; // To track if the player is dead

    public GameObject gameOverPanel; // Reference to the GameOver panel

    public AudioClip deathSound; // Add this field to assign the death sound clip
    private AudioSource audioSource; // Add an AudioSource component

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

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
        if (!isDead && !animator.GetBool("win") && (collision.gameObject.CompareTag("Shark") || collision.gameObject.CompareTag("Sea") || collision.gameObject.CompareTag("Sparrow")))
        {
            // Check if the player has not won and the collision is with the shark, sea, or sparrow
            isDead = true;
            animator.SetBool("isDead", true); // Set the isDead parameter to true
            rb.velocity = Vector3.zero; // Stop any current movement
            rb.isKinematic = true; // Make the Rigidbody kinematic to prevent further physics interactions
            PlayDeathSound(); // Play the death sound
            ScoreManager.instance.ResetScore();
            StartCoroutine(DelayedDestroy()); // Start coroutine to delay destruction
            Debug.Log("Player hit by shark, sea, or sparrow");
        }
    }

    private void Update()
    {
        if (!isDead && !animator.GetBool("win") && Vector3.Distance(transform.position, shark.position) < 2f)
        {
            // Check if the player has not won and is within a certain distance of the shark
            transform.position = Vector3.MoveTowards(transform.position, shark.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, shark.position) < 0.01f)
            {
                isDead = true;
                animator.SetBool("isDead", true); // Set the isDead parameter to true
                rb.velocity = Vector3.zero; // Stop any current movement
                rb.isKinematic = true; // Make the Rigidbody kinematic to prevent further physics interactions
                PlayDeathSound(); // Play the death sound
                ScoreManager.instance.ResetScore();
                StartCoroutine(DelayedDestroy()); // Start coroutine to delay destruction
                Debug.Log("Player caught by shark");
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

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(gameOverDelay); // Wait before showing the GameOver panel
        ShowGameOverPanel(); // Show the GameOver panel
        yield return new WaitForSeconds(deathDelay); // Wait for the specified delay
        Destroy(gameObject); // Destroy the game object after the delay
    }
}
