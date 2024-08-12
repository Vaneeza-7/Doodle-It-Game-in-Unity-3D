using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GemCollectionEffect : MonoBehaviour
{
    public TextMeshProUGUI gemText;
    public float displayDuration = 1.5f;
    public float fadeSpeed = 2f;
    private Color originalColor;

    private void Start()
    {
        originalColor = gemText.color;
        gemText.enabled = false; // Hide text at start
    }

    public void ShowGemCollected()
    {
        gemText.enabled = true;
        gemText.color = originalColor; // Reset color
        StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < displayDuration)
        {
            elapsedTime += Time.deltaTime;
            gemText.transform.Translate(Vector3.up * Time.deltaTime); // Move text upwards
            gemText.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, elapsedTime / displayDuration));
            yield return null;
        }

        gemText.enabled = false; // Hide text after fade out
    }
}
