using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public DrawWithTouch drawControllTouch;
    bool startMovement = false;

    public float speed = 10f;

    Vector3[] positionsTouch;
    int moveIndex = 0;

    private Rigidbody rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Get the Animator component
        rb.useGravity = false; // Disable gravity at the start
    }

    private void OnMouseDown()
    {
        if (animator.GetBool("isDead")) return; // Check if the player is dead
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                drawControllTouch.StartLine(hit.point);
            }
        }
    }

    private void OnMouseDrag()
    {
        if (animator.GetBool("isDead")) return; // Check if the player is dead
        drawControllTouch.UpdateLine();
    }

    private void OnMouseUp()
    {
        if (animator.GetBool("isDead")) return; // Check if the player is dead
        positionsTouch = new Vector3[drawControllTouch.line.positionCount];
        drawControllTouch.line.GetPositions(positionsTouch);
        startMovement = true;
        moveIndex = 0;
        rb.useGravity = false; // Disable gravity when starting to move
        animator.SetFloat("Speed", speed); // Set speed to movement speed when movement starts
    }

    private void Update()
    {
        if (animator.GetBool("isDead")) // Check if the player is dead
        {
            ResetMovement();
            return;
        }

        if (startMovement)
        {
            if (positionsTouch == null || positionsTouch.Length == 0)
            {
                startMovement = false;
                rb.useGravity = true; // Enable gravity if there are no positions
                animator.SetFloat("Speed", 0); // Set speed to 0 when movement ends
                return;
            }

            Vector3 currentPosTouch = positionsTouch[moveIndex];
            transform.position = Vector3.MoveTowards(transform.position, currentPosTouch, speed * Time.deltaTime);

            float distanceTouch = Vector3.Distance(transform.position, currentPosTouch);
            if (distanceTouch < 0.01f)
            {
                moveIndex++;
            }
            if (moveIndex == positionsTouch.Length)
            {
                startMovement = false;
                rb.useGravity = true; // Enable gravity when movement ends
                animator.SetFloat("Speed", 0); // Set speed to 0 when movement ends
            }
        }
        else
        {
            animator.SetFloat("Speed", 0); // Set speed to 0 when not moving
        }
    }

    public void ResetMovement()
    {
        startMovement = false;
        positionsTouch = null;
        moveIndex = 0;
        rb.useGravity = true; // Enable gravity when movement is reset
        animator.SetFloat("Speed", 0); // Set speed to 0 when movement is reset
    }
}
