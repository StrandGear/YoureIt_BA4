using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hands : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [Header("Animations Settings")]
    [SerializeField] private float attackMoveTime = 1.5f;
    [SerializeField] private float attackAnimationDelay = 1.5f;
    [SerializeField] private float returnMoveTime = 1.5f;
    
    private Vector3 origin;
    private bool isActive;

    private bool isAnimating;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance < 8 && !isActive)
        {
            isActive = true;
            StartCoroutine(HandAttack());
            print("Player Detected");
        }
        else if (distance > 8 && isActive && !isAnimating)
        {
            isActive = false;
            var x = origin.x;
            LeanTween.cancel(gameObject);
            LeanTween.moveX(gameObject, x, 1.5f);
            
        }
        
    }

    IEnumerator HandAttack()
    {
        isAnimating = true;
        var x = origin.x + 5;
        LeanTween.cancel(gameObject);
        LeanTween.moveX(gameObject, x, attackMoveTime);
        yield return new WaitForSeconds(attackAnimationDelay);
        var start = transform.position;
        for (int i = 0; i < 3; i++)
        {
            //print("Attack" + i);
            var pos = transform.position + new Vector3(Random.Range(-3f, 3f), -2f, Random.Range(-3f, 3f));
            LeanTween.move(gameObject, pos, 1f);
            yield return new WaitForSeconds(1f);
            LeanTween.move(gameObject, start, returnMoveTime);
            yield return new WaitForSeconds(1f);

        }

        isAnimating = false;
    }
}
