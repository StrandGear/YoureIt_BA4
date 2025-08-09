using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SOS_RespawnPlayerOnKeyPressed : MonoBehaviour
{
    [SerializeField] private InputActionReference respawnKey;
    [SerializeField] private InputActionReference addLockerKey;
    public PlayerRespawn playerRespawn;

    private void OnEnable()
    {
        respawnKey.action.Enable();
        addLockerKey.action.Enable();
    }

    private void OnDisable()
    {
        respawnKey.action.Disable();
        addLockerKey.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (respawnKey == null)
            return;

        else
        {
            if (respawnKey.action.WasPressedThisFrame())
            {
                print("Respawn pressed");
                playerRespawn?.RespawnPlayerAndShowDeathScreen();
            }
        }


        if (addLockerKey == null)
            return;
        else
        {
                if (addLockerKey.action.WasPressedThisFrame())
                {
                    print("Add key");
                    PlayerInventory.Instance.AddKey();
                }
        }
    }
}
