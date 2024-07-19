using UnityEngine;
using System.Collections;

public class RandomEmissionIntensity : MonoBehaviour
{
    public Renderer objectRenderer; // Assign the object with the emission material in the Inspector
    public float minEmission = 0f; // Minimum emission intensity
    public float maxEmission = 6f; // Maximum emission intensity
    public float minFlickerTime = 0.1f; // Minimum time between flickers
    public float maxFlickerTime = 0.5f; // Maximum time between flickers
    public Color emissionColor = Color.white; // Base emission color

    private Material material;
    private Coroutine flickerCoroutine;

    void Start()
    {
        if (objectRenderer == null)
        {
            objectRenderer = GetComponent<Renderer>();
        }
        
        material = objectRenderer.material;

        if (material.HasProperty("_EmissionColor"))
        {
            material.EnableKeyword("_EMISSION");
        }

        flickerCoroutine = StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            float emission = Random.Range(minEmission, maxEmission);
            Color finalColor = emissionColor * Mathf.LinearToGammaSpace(emission);
            material.SetColor("_EmissionColor", finalColor);

            float waitTime = Random.Range(minFlickerTime, maxFlickerTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
