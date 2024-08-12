using UnityEngine;
using System.Collections;

public class LightningEffect : MonoBehaviour
{
    public Transform blockA;
    public Transform blockB;
    private LineRenderer lineRenderer;
    public float duration = 5f;
    public float heightAboveBlocks = 10f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(GenerateLightning());
    }

    IEnumerator GenerateLightning()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the midpoint between the blocks
            Vector3 midpoint = (blockA.position + blockB.position) / 2;
            // Calculate the start point above the midpoint
            Vector3 startPoint = midpoint + Vector3.up * heightAboveBlocks;

            // Set the positions of the line renderer
            lineRenderer.positionCount = 3;
            lineRenderer.SetPosition(0, startPoint);

            // Add randomness to the lightning effect at the midpoint
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f)
            );
            Vector3 randomMidpoint = midpoint + randomOffset;

            lineRenderer.SetPosition(1, randomMidpoint);
            lineRenderer.SetPosition(2, midpoint);

            yield return new WaitForSeconds(0.1f); // Short delay to simulate flickering effect
        }

        // Disable the line renderer after the duration
        lineRenderer.enabled = false;
    }
}
