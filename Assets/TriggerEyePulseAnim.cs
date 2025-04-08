using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEyePulseAnim : MonoBehaviour
{
    [SerializeField] Animator eyeAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eyeAnimator.SetBool("StartPulsating", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eyeAnimator.SetBool("StartPulsating", false);
        }
    }
}
