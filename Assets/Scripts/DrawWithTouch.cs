using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithTouch : MonoBehaviour
{
    public LineRenderer line;
    private Vector3 touchPos; // Changed to Vector3 to store the correct z-position
    private Vector3 lastPos;  // Changed to Vector3 to store the correct z-position
    private int vertexCount = 0;
    public float lineWidth = 0.3f; // Set consistent line width for visibility
    public float zPosition = 1.0f; // Z position for the line, ensure it's in front of the camera

    private Camera mainCamera;

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

        // Ensure the line has a visible material
        if (line.material == null)
        {
            Material defaultMaterial = new Material(Shader.Find("Sprites/Default"));
            defaultMaterial.color = Color.red; // Set to red or any visible color
            line.material = defaultMaterial;
        }

        // Find and validate the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Ensure your camera is tagged as 'MainCamera'.");
        }

        Debug.Log("DrawWithTouch script initialized.");
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

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 screenPosition = new Vector3(touch.position.x, touch.position.y, Mathf.Abs(mainCamera.transform.position.z) + zPosition); // Use zPosition to set z coordinate

            // Detect touch start and initialize drawing
            if (touch.phase == TouchPhase.Began)
            {
                touchPos = mainCamera.ScreenToWorldPoint(screenPosition);

                Debug.Log($"Touch Began at {touchPos}");

                line.positionCount = 1;
                line.SetPosition(0, touchPos);
                vertexCount = 1;
                lastPos = touchPos;

                // Play drawing sound
                PlayDrawingSound();
            }
            // Continue drawing while touch is moving
            else if (touch.phase == TouchPhase.Moved)
            {
                touchPos = mainCamera.ScreenToWorldPoint(screenPosition);

                Debug.Log($"Touch Moved at {touchPos}");

                if (Vector3.Distance(touchPos, lastPos) > 0.05f)
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
                        line.SetPosition(vertexCount, touchPos);
                        vertexCount++;
                        lastPos = touchPos;
                        Debug.Log($"Line drawn to {touchPos}");

                        // Play drawing sound
                        //PlayDrawingSound();
                    }
                }
            }
            // Clear the line when the touch ends
            else if (touch.phase == TouchPhase.Ended)
            {
                // Clear();
            }
        }
    }

    public void Clear()
    {
        if (line == null) return;

        line.positionCount = 0;
        vertexCount = 0;
        Debug.Log("Line cleared");
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
