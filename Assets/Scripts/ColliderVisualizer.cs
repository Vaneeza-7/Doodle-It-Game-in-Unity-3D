using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class ColliderVisualizer : MonoBehaviour
{
    private MeshCollider meshCollider;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void OnDrawGizmos()
    {
        if (meshCollider != null && meshCollider.sharedMesh != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireMesh(meshCollider.sharedMesh, transform.position, transform.rotation, transform.localScale);
        }
    }
}

