using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float xCoInput = inputHandler.MoveInput.x;
        float zCoInput = inputHandler.MoveInput.y;  

        Vector3 move = new Vector3(xCoInput, 0, zCoInput);
        
        controller.Move(move * (Time.deltaTime * playerSpeed));

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (inputHandler.JumpInput && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}