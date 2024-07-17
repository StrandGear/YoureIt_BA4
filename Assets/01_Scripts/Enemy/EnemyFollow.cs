using System;
using UnityEngine;

public class EnemyFollow
    : MonoBehaviour
{
    [SerializeField] Transform target; // The player or target to follow
    [SerializeField] float rangeToFollow = 10f; // Range within which the enemy starts following
    [SerializeField] float attackRange = 2f; // Range within which the enemy starts attacking
    [SerializeField] float moveSpeed = 5f; // Movement speed when following
    [SerializeField] float attackingSpeed = 7f; // Movement speed when attacking
    [SerializeField]  PlayerRespawn playerRespawn; 
    [SerializeField]  float respawnDistance; 

    private Vector3 targetStoredPosition; 
    private bool attack = false; 
    private bool isFollowingThePlayer = false; 
    private float distance;

    private void OnEnable()
    {
        playerRespawn.onPlayerRespawn += ResetEnemy;
    }

    private void ResetEnemy()
    {
        transform.position = target.position + Vector3.right * (rangeToFollow + respawnDistance);
        Debug.Log("Enemy Respawn");
    }

    private void OnDisable()
    {
        playerRespawn.onPlayerRespawn -= ResetEnemy;
    }

    void Update()
    {
        var targetPosition = target.position + Vector3.right * rangeToFollow;
        var difference = targetPosition - transform.position;
        var movement = difference.normalized * moveSpeed * Time.deltaTime;
        transform.position += movement;
        var pos = transform.position;
        transform.position = new Vector3(pos.x, targetPosition.y, pos.z);
        /*distance = Vector3.Distance(transform.position, target.position);

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
        }*/
    }
}