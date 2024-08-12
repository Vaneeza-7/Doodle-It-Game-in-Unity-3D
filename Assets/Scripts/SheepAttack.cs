using System.Collections;
using UnityEngine;

public class SheepAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    public GameObject[] sheep; // Array of sheep GameObjects
    public float speed = 10f;
    public Transform[] sheepTargets; // Array of sheep Transforms

    public float deathDelay = 1f; // Time to wait before destroying the game object
    public float gameOverDelay = 2f; // Time to wait before showing the GameOver panel

    private bool isDead = false; // To track if the player is dead
    private Animator playerAnimator;

    public GameObject gameOverPanel; // Reference to the GameOver panel
    public AudioClip deathSound; // Add this field to assign the death sound clip
    private AudioSource audioSource; // Add an AudioSource component

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        // Find the player GameObject and get its Animator component
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
        }

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        if (sheep == null || sheep.Length == 0)
        {
            Debug.LogError("Sheep array is not assigned or empty!");
            return;
        }

        for (int i = 0; i < sheep.Length; i++)
        {
            if (sheep[i] == null)
            {
                Debug.LogError("Sheep at index " + i + " is null!");
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
        if (!isDead && !playerAnimator.GetBool("win") &&
            (collision.gameObject.CompareTag("Shark") ||
             collision.gameObject.CompareTag("Sea") ||
             collision.gameObject.CompareTag("Sparrow") ||
             collision.gameObject.CompareTag("Sheep")))
        {
            isDead = true;
            Debug.Log("Sheep attacked");
            animator.SetBool("isDead", true); // Set the isDead parameter to true
            PlayDeathSound(); // Play the death sound
            rb.velocity = Vector3.zero; // Stop any current movement
            rb.isKinematic = true; // Make the Rigidbody kinematic to prevent further physics interactions
            StartCoroutine(HandleDeath()); // Start coroutine to handle death and show the game over panel
        }
    }

    private void Update()
    {
        if (!isDead && !playerAnimator.GetBool("win"))
        {
            for (int i = 0; i < sheepTargets.Length; i++)
            {
                if (Vector3.Distance(transform.position, sheepTargets[i].position) < 2f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, sheepTargets[i].position, speed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, sheepTargets[i].position) < 0.01f)
                    {
                        isDead = true;
                        Debug.Log("Sheep attacked by coming to you");
                        animator.SetBool("isDead", true); // Set the isDead parameter to true
                        PlayDeathSound(); // Play the death sound
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
