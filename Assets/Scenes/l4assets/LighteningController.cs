using UnityEngine;
using System.Collections;

public class LightningStrike : MonoBehaviour
{
    public GameObject lightningPrefab;  // The lightning asset prefab
    public float interval = 5f;         // Time interval between strikes
    public GameObject fx_Lightening_05;   // The fx_lightening_05 game object

    void Start()
    {
        StartCoroutine(SpawnLightning());
    }

    IEnumerator SpawnLightning()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(interval); // Wait for the interval before the next strike
        }
    }

    void Spawn()
    {
        // Define the spawn position at the top of the screen
        Vector3 spawnPosition = new Vector3(0, Camera.main.orthographicSize, 0);

        // Instantiate the lightning prefab
        GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);

        // Move the lightning to the center over time
        StartCoroutine(MoveLightning(lightning));
    }

    IEnumerator MoveLightning(GameObject lightning)
    {
        Vector3 targetPosition = new Vector3(0, 0, 0);
        float duration = 0.5f;  // Duration of the strike animation
        float elapsed = 0f;

        while (elapsed < duration)
        {
            lightning.transform.position = Vector3.Lerp(lightning.transform.position, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        lightning.transform.position = targetPosition;

        // Wait for 5 seconds before destroying the lightning object
        yield return new WaitForSeconds(5f);

        // Destroy the fx_lightening_05 game object after 5 seconds
        Destroy(fx_Lightening_05);
    }
}