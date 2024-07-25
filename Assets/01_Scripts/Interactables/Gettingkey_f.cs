using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Gettingkey_f : MonoBehaviour
{
    bool wasPressed = false;
    [SerializeField] private InputActionReference button;
    private void OnEnable()
    {
        button.action.Enable();
    }

    private void OnDisable()
    {
        button.action.Disable();
    }

    private void Update()
    {
        if (button.action.WasPressedThisFrame())
        {
            wasPressed = true;
        }
    }
        private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null && wasPressed)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Keys, gameObject.transform.position);

            PlayerInventory.Instance.AddKey();

            Destroy(gameObject, 0.5f);
        }
    }
}
