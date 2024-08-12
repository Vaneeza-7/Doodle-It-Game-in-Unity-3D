using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public MeshCollider meshCollider;

    public Rigidbody rb;

    [HideInInspector] public List<Vector3> points = new List<Vector3>();
    [HideInInspector] public int pointsCount = 0;

    private float pointsMinDistance = 0.1f;
    private float CircleColliderRadius;

    public Vector3 GetLastPoint()
    {
        return (Vector3)lineRenderer.GetPosition(pointsCount - 1); 
    }

    public void UsePhysics(bool usePhysics)
    {
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = !usePhysics;
        Debug.Log("Physics enabled: " + !rb.isKinematic);
    }

    public void SetLineColor(Gradient lineColor)
    {
        lineRenderer.colorGradient = lineColor;
    }

    public void SetLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        //CircleColliderRadius = width / 2;
        UpdateMeshCollider();
    }

    public void AddPoint(Vector3 newPoint)
    {
        if (pointsCount >= 1 && Vector3.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
            return;

        points.Add(newPoint);
        pointsCount++;

        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(pointsCount - 1, newPoint);

        if(pointsCount>1)
        {
           //for 2d: edgeCollider.points = points.ToArray();
           UpdateMeshCollider();
        }
        UpdateMeshCollider();
    }

    private void UpdateMeshCollider()
    {
        if(meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
            Debug.Log("Mesh collider added");
        }

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);
       // meshCollider.sharedMesh = mesh;

       // meshCollider.convex = true; 
       // meshCollider.isTrigger = false; 

        // Generate normals for the mesh
        if (mesh.vertexCount > 0)
        {
            mesh.RecalculateNormals();
            meshCollider.sharedMesh = mesh;

            meshCollider.convex = true;
            meshCollider.isTrigger = false;

            //Debug.Log("Mesh collider updated with " + mesh.vertexCount + " vertices and " + (mesh.triangles.Length / 3) + " triangles.");
            DebugMesh(mesh);
        }
        else
        {
            Debug.LogWarning("Mesh has no vertices. Collider not updated.");
        }

       // Debug.Log("Mesh collider updated");
        //DebugMesh(mesh);
    }

    public void SetPointMinDistance(float distance)
    {
        pointsMinDistance = distance;
    }

    private void DebugMesh(Mesh mesh)
    {
        Debug.Log("Mesh vertices count: " + mesh.vertexCount);
        Debug.Log("Mesh triangles count: " + mesh.triangles.Length / 3);

        // Visualize the mesh in the editor
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Debug.DrawLine(mesh.vertices[i], mesh.vertices[i] + Vector3.up * 0.1f, Color.red, 2f);
        }
    }

    private void OnDrawGizmos()
    {
        if (meshCollider != null && meshCollider.sharedMesh != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireMesh(meshCollider.sharedMesh, transform.position, transform.rotation);
        }
    }

     private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Ongoing collision with " + collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision ended with " + collision.gameObject.name);
    }

}
