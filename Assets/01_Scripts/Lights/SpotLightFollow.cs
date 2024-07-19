using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;   // Reference to the player's transform
    public Vector3 offset;     // Offset distance between the player and spotlight

    void Start()
    {
        // Optionally initialize the offset here
        // offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Set the position of the spotlight to the player's position plus the offset
        transform.position = player.position + offset;
    }
}

