using UnityEngine;

public class FogArea : MonoBehaviour
{
    public float targetFogDensity = 0.1f; // Target fog density inside the area
    public Color targetFogColor = Color.gray; // Target fog color inside the area
    public float transitionSpeed = 1f;    // Speed of transition to new fog density and color

    private float defaultFogDensity;
    private Color defaultFogColor;

    void Start()
    {
        // Store the default fog settings to reset them when the player exits the area
        defaultFogDensity = RenderSettings.fogDensity;
        defaultFogColor = RenderSettings.fogColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it only affects the player
        {
            StopAllCoroutines(); // Stop any ongoing transitions
            StartCoroutine(ChangeFog(targetFogDensity, targetFogColor));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(ChangeFog(defaultFogDensity, defaultFogColor));
        }
    }

    private System.Collections.IEnumerator ChangeFog(float targetDensity, Color targetColor)
    {
        float startDensity = RenderSettings.fogDensity;
        Color startColor = RenderSettings.fogColor;
        float progress = 0f;

        while (progress < 1f)
        {
            // Interpolate fog density and color over time
            RenderSettings.fogDensity = Mathf.Lerp(startDensity, targetDensity, progress);
            RenderSettings.fogColor = Color.Lerp(startColor, targetColor, progress);

            progress += Time.deltaTime * transitionSpeed;
            yield return null; // Wait for the next frame
        }

        // Ensure final values are set
        RenderSettings.fogDensity = targetDensity;
        RenderSettings.fogColor = targetColor;
    }
}
