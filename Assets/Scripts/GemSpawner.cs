using UnityEngine;
using System.Collections;

public class GemSpawner : MonoBehaviour
{
    public GameObject[] gemPrefabs; // Array of gem prefabs
    public float spawnHeight = 7.35f; // Fixed height (Y value)
    public float spawnX = 62.31f; // Fixed X value
    public float minZ = -10f; // Minimum Z value for spawning
    public float maxZ = -1f; // Maximum Z value for spawning
    public float spawnInterval = 0.05f; // Time between spawns
    public float levelDuration = 100f; // Duration of the level

    private float timer;

    void Start()
    {
        timer = levelDuration;
        StartCoroutine(SpawnGems());
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StopAllCoroutines(); // Stop spawning gems
            Debug.Log("Level ended.");
        }
    }

    IEnumerator SpawnGems()
    {
        while (timer > 0)
        {
            SpawnGem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnGem()
    {
        int randomIndex = Random.Range(0, gemPrefabs.Length);
        GameObject gem = Instantiate(gemPrefabs[randomIndex]);

        // Random spawn position within the specified Z range
        Vector3 spawnPosition = new Vector3(
            spawnX, // Fixed X position
            spawnHeight, // Fixed Y position
            Random.Range(minZ, maxZ)  // Random Z position within the range
        );

        gem.transform.position = spawnPosition;
    }
}
