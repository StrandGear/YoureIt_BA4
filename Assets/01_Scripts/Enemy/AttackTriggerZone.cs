using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerZone : MonoBehaviour
{
    public EnemyAttack enemyAttack; 
    public float killedTimer = 5.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAttack.PlayerEntered();
            print("Player In");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAttack.PlayerExited(); // Notify EnemyAttack that the player exited
            print("Player Out");
        }
    }
}