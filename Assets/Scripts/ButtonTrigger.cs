using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject wall; // Assign the wall GameObject in the Inspector
    public Vector3 newWallPosition; // The position where the wall should move to

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Move the wall to the new position
            wall.transform.position = newWallPosition;
        }
    }
}