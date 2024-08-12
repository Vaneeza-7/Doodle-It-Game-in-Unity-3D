using System.Collections;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public float moveDistance = 10f; // The distance the bird will move back and forth
    public float sideDistance = 6f; // The distance the bird will move to the side
    public float speed = 2f; // The speed of the bird's movement
    public float rotationSpeed = 5f; // Speed of rotation
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool movingToEnd = true;

    private void Start()
    {
        startPosition = transform.position;
        //z axis
        endPosition = startPosition + transform.forward * sideDistance;
        StartCoroutine(MoveBird());
    }

    private IEnumerator MoveBird()
    {
        while (true)
        {
            if (movingToEnd)
            {
                MoveAndRotateTowards(endPosition);
                if (Vector3.Distance(transform.position, endPosition) < 0.01f)
                {
                    movingToEnd = false;
                }
            }
            else
            {
                MoveAndRotateTowards(startPosition);
                if (Vector3.Distance(transform.position, startPosition) < 0.01f)
                {
                    movingToEnd = true;
                }
            }
            yield return null; // Wait until the next frame
        }
    }

    private void MoveAndRotateTowards(Vector3 targetPosition)
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Calculate the direction to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate the rotation step
        if (direction != Vector3.zero) // Ensure there is a direction to look at
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            //Debug.Log("Rotating towards " + targetPosition + " with direction " + direction);
        }
    }
}
