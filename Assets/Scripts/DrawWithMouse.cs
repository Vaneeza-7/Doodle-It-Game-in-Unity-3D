using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public LineRenderer line;
    private Vector3 mousePos; // Changed to Vector3 to store the correct z-position
    private Vector3 lastPos;  // Changed to Vector3 to store the correct z-position
    private int vertexCount = 0;
    public float lineWidth = 0.3f; // Set consistent line width for visibility
    public float zPosition = 1.0f; // Z position for the line, ensure it's in front of the camera

    private Camera mainCamera;

    [SerializeField] private float minDistance = 0.01f;

    public LayerMask CantDrawOverLayer;
    int CantDrawOverLayerIndex;

    public Player player;

    public AudioClip drawSound; // Add this field to assign the sound clip
    private AudioSource audioSource; // Add an AudioSource component

    public AudioClip cantDrawOverSound; // Add this field to assign the sound clip

    void Start()
    {
        // Ensure LineRenderer is properly initialized
        line = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        if (line == null)
        {
            Debug.LogError("LineRenderer component not found.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found.");
            return;
        }

        // Set initial properties for the LineRenderer
        line.positionCount = 0;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.useWorldSpace = true; // Ensure using world space
        CantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");

        // Find and validate the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Ensure your camera is tagged as 'MainCamera'.");
        }

        Debug.Log("DrawWithMouse script initialized.");
        Debug.Log($"LineRenderer initial position: {line.transform.position}");
    }

    public void StartLine(Vector3 position)
    {
        line.positionCount = 1;
        line.SetPosition(0, position);
        vertexCount = 1;
        lastPos = position;

        // Play drawing sound
        PlayDrawingSound();
    }

    public void UpdateLine()
    {
        if (line == null || mainCamera == null) return;

        // Detect mouse button down and initialize drawing
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(mainCamera.transform.position.z) + zPosition); // Use zPosition to set z coordinate
            mousePos = mainCamera.ScreenToWorldPoint(screenPosition);

            Debug.Log($"Mouse Button Down at {mousePos}");

            line.positionCount = 1;
            line.SetPosition(0, mousePos);
            vertexCount = 1;
            lastPos = mousePos;

            // Play drawing sound
            PlayDrawingSound();
        }
        
        // Continue drawing while mouse button is held down
        else if (Input.GetMouseButton(0))
        {
            Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(mainCamera.transform.position.z) + zPosition); // Use zPosition to set z coordinate
            mousePos = mainCamera.ScreenToWorldPoint(screenPosition);

            Debug.Log($"Mouse Button Held at {mousePos}");

            if (Vector3.Distance(mousePos, lastPos) > 0.05f)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, CantDrawOverLayer))
                {
                    Debug.Log("Hit detected, cannot draw");
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                    // Reset the line if it hits an object in CantDrawOverLayer
                    ResetLine();
                    player.ResetMovement();
                     // Play cant draw over sound
                    PlayCantDrawOverSound();
                    
                }
                else
                {
                    Debug.Log("No hit detected, drawing line");
                    Debug.DrawRay(ray.origin, ray.direction * 20f, Color.black);

                    line.positionCount = vertexCount + 1;
                    line.SetPosition(vertexCount, mousePos);
                    vertexCount++;
                    lastPos = mousePos;
                    Debug.Log($"Line drawn to {mousePos}");

                    // Play drawing sound
                   // PlayDrawingSound();
                }
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Button Up. Drawing complete.");
        }
    }

    public void ResetLine()
    {
        line.positionCount = 0;
        vertexCount = 0;
    }

    private void PlayDrawingSound()
    {
        if (audioSource != null && drawSound != null)
        {
            audioSource.PlayOneShot(drawSound);
        }
    }

    private void PlayCantDrawOverSound()
    {
        if (audioSource != null && cantDrawOverSound != null)
        {
            audioSource.PlayOneShot(cantDrawOverSound);
        }
    }
}
