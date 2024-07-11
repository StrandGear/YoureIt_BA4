using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;        // The animator component
    public Transform targetPosition; // The target position for the animation model to move to
    public float moveSpeed = 2f;     // Speed at which the model moves
    private Vector3 initialPosition; // Initial position of the model
    private bool isPlayerInside = false;

    void Start()
    {
        initialPosition = transform.position; // Save the initial position of the model
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("HandAttack", false); // Ensure the animation is off at the start
    }

    public void PlayerEntered()
    {
        isPlayerInside = true;
        StopAllCoroutines(); // Stop any ongoing movement coroutine
        StartCoroutine(MoveToPosition(targetPosition.position)); // Start moving the model forward
        print("Enemy Attack");
    }

    public void PlayerExited()
    {
        isPlayerInside = false;
        StopAllCoroutines(); // Stop any ongoing movement coroutine
        StartCoroutine(MoveToPosition(initialPosition)); // Start moving the model backward
        print("Enemy Retreat");
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target; // Snap to the target position

        if (isPlayerInside)
        {
            animator.SetBool("HandAttack", true); // Play the animation when reached the forward target
        }
        else
        {
            animator.SetBool("HandAttack", false); // Stop the animation when moving backward
        }
    }
}