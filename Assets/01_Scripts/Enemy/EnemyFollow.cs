using System;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] Transform target; 
    [SerializeField] float rangeToFollow = 10f; 
    [SerializeField] float attackRange = 2f; 
    [SerializeField] float moveSpeed = 5f; 
    [SerializeField] float attackingSpeed = 7f;
    [SerializeField] float speedUpDistance = 20f; // Distance to speed up
    [SerializeField] float respawnDistance;
    [SerializeField] PlayerRespawn playerRespawn; 
     
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
        float currentSpeed = moveSpeed;
        distance = Vector3.Distance(transform.position, target.position + Vector3.right * rangeToFollow);

        if (distance > speedUpDistance)
        {
            currentSpeed = attackingSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        var targetPosition = target.position + Vector3.right * rangeToFollow;
        var difference = targetPosition - transform.position;
        var movement = difference.normalized * currentSpeed * Time.deltaTime;
        transform.position += movement;
        var pos = transform.position;
        transform.position = new Vector3(pos.x, targetPosition.y, pos.z);
    }
}