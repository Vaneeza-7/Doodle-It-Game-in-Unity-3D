using UnityEngine;

public class buttonpress : MonoBehaviour
{
    public GameObject topCube; // Reference to the top cube
    public Animator topCubeAnimator; // Reference to the Animator on the top cube
    public string buttonPressTrigger = "Press"; // Name of the trigger in the Animator

    public AudioClip buttonPressSound; // Add this field to assign the button press sound clip
    private AudioSource audioSource; // Add an AudioSource component

    private void Start()
    {
        if (topCube == null)
        {
            Debug.LogError("Top Cube is not assigned!");
        }

        if (topCubeAnimator == null)
        {
            Debug.LogError("Top Cube Animator is not assigned!");
        }

        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Trigger the button press animation
            topCubeAnimator.SetTrigger(buttonPressTrigger);

            // Play the button press sound
            PlayButtonPressSound();

            Debug.Log("Button Pressed");
        }
    }

    private void PlayButtonPressSound()
    {
        if (audioSource != null && buttonPressSound != null)
        {
            audioSource.PlayOneShot(buttonPressSound);
        }
    }
}
