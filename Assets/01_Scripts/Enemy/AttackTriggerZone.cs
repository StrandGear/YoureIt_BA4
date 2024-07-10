using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerZone : MonoBehaviour
{
    public EnemyAttack enemyAttack; // Reference to the EnemyAttack script

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAttack.PlayerEntered(); // Notify EnemyAttack that the player entered
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