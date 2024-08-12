using System.Collections;
using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    public float moveDistance = 10f; // The distance the sheep will move back and forth
    public float sideDistance = 6f; // The distance the sheep will move to the side
    public float speed = 2f; // The speed of the sheep's movement
    public float rotationSpeed = 3f; // Speed of rotation
    public float startDelay = 5f; // Delay before the sheep starts moving

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool movingToEnd = true;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + transform.forward * sideDistance; // Move along the z-axis
        StartCoroutine(StartMovingAfterDelay(startDelay));
    }

    private IEnumerator StartMovingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(MoveSheep());
    }

    private IEnumerator MoveSheep()
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

           // Debug.Log("Rotating towards " + targetPosition + " with direction " + direction);
        }
    }
}
