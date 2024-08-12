using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform goldTransform; // Reference to the gold object's transform
    public float speed = 5f; // Movement speed
    public Material lineMaterial; // Reference to the hand-drawn line material

    private LineRenderer lineRenderer;
    private Vector3[] positions;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        positions = new Vector3[2];
        positions[0] = transform.position;
        positions[1] = goldTransform.position;

        // Set up LineRenderer
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, positions[0]);
        lineRenderer.SetPosition(1, positions[1]);

        // Apply the hand-drawn material to the LineRenderer
        lineRenderer.material = lineMaterial;

        StartCoroutine(MoveToGold());
    }

    IEnumerator MoveToGold()
    {
        while (Vector3.Distance(transform.position, goldTransform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goldTransform.position, speed * Time.deltaTime);
            yield return null;
        }

        // Optional: Do something when the capsule reaches the gold
        Debug.Log("Gold Collected!");

        // Clear or disable the LineRenderer
        lineRenderer.positionCount = 0;
        // Alternatively, you can disable the LineRenderer component
        // lineRenderer.enabled = false;

        // Remove the gold object
        Destroy(goldTransform.gameObject);
    }
}