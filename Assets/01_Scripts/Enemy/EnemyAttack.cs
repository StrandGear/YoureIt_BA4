using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;        
    private bool isPlayerInside = false;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("HandAttack", false); 
    }

    public void PlayerEntered()
    {
        isPlayerInside = true;
        animator.SetBool("HandAttack", true); 
        print("Enemy Attack");
    }

    public void PlayerExited()
    {
        isPlayerInside = false;
        animator.SetBool("HandAttack", false); 
        animator.Update(0f);
        print("Enemy Retreat");
    }
}