using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trail : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed = 1.0f;

    private float startTime;
    private float journeyLength;

    void Start()
    {
        // Set the start position and calculate the journey length
        transform.position = startPosition;
        journeyLength = Vector3.Distance(startPosition, endPosition);
        startTime = Time.time;
    }

    void Update()
    {
        // Calculate the distance covered based on the speed and time
        float distCovered = (Time.time - startTime) * speed;
        // Calculate the fraction of the journey completed
        float fractionOfJourney = distCovered / journeyLength;
        // Set the position of the object to the interpolated position
        transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);

        // To make the movement continuous, reset the startTime when the object reaches the end position
        if (fractionOfJourney >= 1.0f)
        {
            // Swap start and end positions to make it move back and forth
            Vector3 temp = startPosition;
            startPosition = endPosition;
            endPosition = temp;

            // Reset the start time and recalculate the journey length
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPosition, endPosition);
        }
    }
}
