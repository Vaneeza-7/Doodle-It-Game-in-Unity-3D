using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject LinePrefabs;
    public LayerMask CantDrawOverLayer;

    int CantDrawOverLayerIndex;

    public float LinePointsMinDistance;
    public float LineWidth;

   // public GameObject BridgePrefab; // Add a reference to the bridge prefab

    public Gradient lineColor;

    Line currentLine;

    private void Start()
    {
        CantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            BeginDraw();
        }
        
        if(currentLine != null)
        {
            Draw();
        }

        if(Input.GetMouseButtonUp(0))
        {
            EndDraw();
        }
    }

    private void BeginDraw()
    {
        currentLine = Instantiate(LinePrefabs, Vector3.zero, Quaternion.identity).GetComponent<Line>();
        currentLine.SetLineColor(lineColor);
        currentLine.SetLineWidth(LineWidth);
        currentLine.SetPointMinDistance(LinePointsMinDistance);
        currentLine.UsePhysics(false);

    }

private void Draw()
{
   // Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z));
    //Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPosition);

// Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         RaycastHit hit;
//         Vector3 mousePos;

//         // Perform a raycast to find the exact world position based on the screen position
//         if (Physics.Raycast(ray, out hit, Mathf.Infinity))
//         {
//             mousePos = hit.point;
//         }
//         else
//         {
//             // If no object is hit, set a default depth
//             mousePos = ray.GetPoint(10); // Adjust the depth as needed
//         }

//     //RaycastHit hit;
//     bool hitSomething = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f, CantDrawOverLayer);

//     if (hitSomething)
//     {
//         Debug.Log("Hit detected");
//         Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.yellow);
//         EndDraw();
//     }
//     else
//      {
//         Debug.Log("No hit detected");
//         Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 20f, Color.black);
//         currentLine.AddPoint(mousePos);
//       }

// Convert screen position to world position using a raycast to determine the exact depth
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePos;

        // Perform a raycast to find the exact world position based on the screen position
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            mousePos = hit.point;
        }
        else
        {
            // If no object is hit, set a default depth based on the camera's view direction
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)); // 10 units from the camera
        }

        // Perform a raycast to check if we hit something in the CantDrawOverLayer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, CantDrawOverLayer))
        {
            Debug.Log("Hit detected");
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
            EndDraw();
        }
        else
        {
            Debug.Log("No hit detected");
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.black);
            currentLine.AddPoint(mousePos);
        }
}


    private void EndDraw()
    {
        if(currentLine != null)
        {
            if(currentLine.pointsCount < 2)
            {
                Destroy(currentLine.gameObject);
            }
            else
            {
                currentLine.gameObject.layer = CantDrawOverLayerIndex;
                currentLine.UsePhysics(true);
                //currentLine = null;
                //Debug.Log("End Drawing");
                // Instantiate the bridge in place of the line
               // InstantiateBridge(currentLine.points);
            }
            currentLine = null;
            Debug.Log("End Drawing");
        }
    }

    public List<Vector3> GetPathPoints()
    {
        if (currentLine != null)
        {
            return new List<Vector3>(currentLine.points);
        }
        return new List<Vector3>();
    }

    // private void InstantiateBridge(List<Vector3> pathPoints)
    // {
    //     GameObject bridge = Instantiate(BridgePrefab, pathPoints[0], Quaternion.identity);
    //     LineRenderer bridgeLineRenderer = bridge.GetComponent<LineRenderer>();

    //     if (bridgeLineRenderer != null)
    //     {
    //         bridgeLineRenderer.positionCount = pathPoints.Count;
    //         bridgeLineRenderer.SetPositions(pathPoints.ToArray());
    //     }

    //     // Optionally, adjust the bridge's scale and orientation to fit the path
    //     AdjustBridgeToPath(bridge, pathPoints);
    // }

    // private void AdjustBridgeToPath(GameObject bridge, List<Vector3> pathPoints)
    // {
    //     // Adjust the bridge's scale and orientation to fit the path points
    //     // This logic will depend on your specific bridge asset and how you want it to fit the path
    //     // Example: Set the scale of the bridge based on the length of the path
    //     float pathLength = Vector3.Distance(pathPoints[0], pathPoints[pathPoints.Count - 1]);
    //     bridge.transform.localScale = new Vector3(pathLength, bridge.transform.localScale.y, bridge.transform.localScale.z);

    //     // Example: Set the rotation of the bridge to match the path direction
    //     Vector3 direction = (pathPoints[pathPoints.Count - 1] - pathPoints[0]).normalized;
    //     bridge.transform.rotation = Quaternion.LookRotation(direction);
    // }

}
