using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float delay = 5.0f;
    public float speed = 5.0f;

    void Start()
    {
       // Invoke("MoveForward", delay);
    }

    public void MoveForward()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float playerWidth = GetPlayerWidth();
        Debug.Log("Player width: " + playerWidth);
        while (true)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            yield return null;
        }
    }

    private float GetPlayerWidth()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            return meshCollider.bounds.size.x;
        }

        Debug.LogError("No MeshCollider found on the player.");
        return 1.0f; // Default value if no collider is found
    }
}
