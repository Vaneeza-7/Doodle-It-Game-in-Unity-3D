using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public DrawWithMouse drawControll;
    bool startMovement = false;

    public float speed = 10f;

    Vector3[] positions;
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
                drawControll.StartLine(hit.point);
            }
        }
    }

    private void OnMouseDrag()
    {
        if (animator.GetBool("isDead")) return; // Check if the player is dead
        drawControll.UpdateLine();
    }

    private void OnMouseUp()
    {
        if (animator.GetBool("isDead")) return; // Check if the player is dead
        positions = new Vector3[drawControll.line.positionCount];
        drawControll.line.GetPositions(positions);
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
            if (positions == null || positions.Length == 0)
            {
                startMovement = false;
                rb.useGravity = true; // Enable gravity if there are no positions
                animator.SetFloat("Speed", 0); // Set speed to 0 when movement ends
                return;
            }

            Vector3 currentPos = positions[moveIndex];
            transform.position = Vector3.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, currentPos);
            if (distance < 0.01f)
            {
                moveIndex++;
            }
            if (moveIndex == positions.Length)
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
        positions = null;
        moveIndex = 0;
        rb.useGravity = true; // Enable gravity when movement is reset
        animator.SetFloat("Speed", 0); // Set speed to 0 when movement is reset
    }
}
