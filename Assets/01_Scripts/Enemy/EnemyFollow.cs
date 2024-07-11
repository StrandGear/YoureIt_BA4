using UnityEngine;

public class EnemyFollow
    : MonoBehaviour
{
    [SerializeField] Transform target; // The player or target to follow
    [SerializeField] float rangeToFollow = 10f; // Range within which the enemy starts following
    [SerializeField] float attackRange = 2f; // Range within which the enemy starts attacking
    [SerializeField] float moveSpeed = 5f; // Movement speed when following
    [SerializeField] float attackingSpeed = 7f; // Movement speed when attacking

    private Vector3 targetStoredPosition; // Position where the player was when attack started
    private bool attack = false; // Flag indicating if the enemy is attacking
    private bool isFollowingThePlayer = false; // Flag indicating if the enemy is following the player
    private float distance; // Distance between the enemy and the player

    void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);

        if (!attack)
        {
            if (distance < rangeToFollow)
            {
                isFollowingThePlayer = true;
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                targetStoredPosition = target.position;

                //Debug.Log("Player's previous position is: " + targetStoredPosition);

                if (distance < attackRange)
                {
                    attack = true;
                }
            }
        }
        else
        {
            isFollowingThePlayer = false;
            transform.position = Vector3.MoveTowards(transform.position, targetStoredPosition, attackingSpeed * Time.deltaTime);

            if (transform.position == targetStoredPosition)
            {
                attack = false;
            }
        }
    }
}