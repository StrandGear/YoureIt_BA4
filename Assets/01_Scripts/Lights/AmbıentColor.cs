using UnityEngine;

public class AmbientColorChanger : MonoBehaviour
{
    public Color startColor = Color.white; // Initial ambient color
    public Color endColor = Color.black; // Final ambient color
    public float startIntensity = 1f; // Initial ambient intensity multiplier
    public float endIntensity = 0.5f; // Final ambient intensity multiplier
    public Transform player; // Reference to the player transform
    public Transform endPointsParent; // Reference to the parent of end point transforms
    private float totalDistance; // Total distance between player start and the closest end point
    private bool playerReachedEnd = false;
    private Transform closestEndPoint; // Closest end point to the player

    void Start()
    {
        // Find the closest end point initially
        closestEndPoint = FindClosestEndPoint();
        totalDistance = Vector3.Distance(player.position, closestEndPoint.position);

        // Set the initial ambient color and intensity multiplier
        RenderSettings.ambientLight = startColor;
        RenderSettings.ambientIntensity = startIntensity;
    }

    void Update()
    {
        if (!playerReachedEnd)
        {
            // Find the current closest end point
            closestEndPoint = FindClosestEndPoint();

            // Calculate the current distance between the player and the closest end point
            float currentDistance = Vector3.Distance(player.position, closestEndPoint.position);

            // Calculate the progress as a value between 0 and 1
            float progress = 1 - (currentDistance / totalDistance);

            // Interpolate the ambient color and intensity based on the player's progress
            RenderSettings.ambientLight = Color.Lerp(startColor, endColor, progress);
            RenderSettings.ambientIntensity = Mathf.Lerp(startIntensity, endIntensity, progress);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerReachedEnd = true;
            RenderSettings.ambientLight = endColor;
            RenderSettings.ambientIntensity = endIntensity;
        }
    }

    // Find the closest end point to the player
    Transform FindClosestEndPoint()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform endPoint in endPointsParent)
        {
            float distance = Vector3.Distance(player.position, endPoint.position);
            if (distance < minDistance)
            {
                closest = endPoint;
                minDistance = distance;
            }
        }

        return closest;
    }

    // Helper method to convert hex code to Color
    private Color HexToColor(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogError("Invalid hex code");
            return Color.white;
        }
    }
}
