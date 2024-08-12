using UnityEngine;

public class BridgeDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform startPoint;  // The point where the player starts drawing
    public Transform endPoint;    // The point where the gold is placed
    private bool isDrawing = false;

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
        {
            isDrawing = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint.position);
        }

        if (isDrawing)
        {
            Vector3 currentPosition;
            if (Input.touchCount > 0)
            {
                currentPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                currentPosition.z = 0;
            }
            else
            {
                currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentPosition.z = 0;
            }

            lineRenderer.SetPosition(1, currentPosition);

            if (Input.GetKeyUp(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                isDrawing = false;
                lineRenderer.SetPosition(1, endPoint.position);
            }
        }
    }
}
