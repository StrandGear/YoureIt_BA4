using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SOS_RespawnPlayerOnKeyPressed : MonoBehaviour
{
    [SerializeField] private InputActionReference respawnKey;
    public PlayerRespawn playerRespawn;

    private void OnEnable()
    {
        respawnKey.action.Enable();
    }

    private void OnDisable()
    {
        respawnKey.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (respawnKey.action.WasPressedThisFrame())
        {
            print("Respawn pressed");
            playerRespawn.RespawnPlayerOtsideTriggerEvents();
        }
    }
}
