using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;
    [SerializeField] private InputActionReference speedBoostControl; 
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float speedBoostMultiplier = 2.0f; 
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;
    
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isClimbing;
    private Transform cameraMainTransform;

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        speedBoostControl.action.Enable(); 
    }
    
    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        speedBoostControl.action.Disable(); 
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0;

        float currentSpeed = playerSpeed;
        if (speedBoostControl.action.IsPressed()) 
        {
            currentSpeed *= speedBoostMultiplier; 
        }

        float avoidFloorDistance = 0.1f;
        float ladderGrabDistance = 0.4f;
        Vector3 raycastStart = transform.position + Vector3.up * avoidFloorDistance;
        if (Physics.Raycast(raycastStart, move, out RaycastHit raycastHit, ladderGrabDistance))
        {
            if (raycastHit.transform.TryGetComponent(out Ladder ladder))
            {
                isClimbing = true;
                move.x = 0f;
                move.z = 0f;
                move.y = movement.y; // Use the y-axis for climbing
                groundedPlayer = true; // Ensure grounded for climbing
                playerVelocity.y = 0f; // Reset vertical velocity
            }
        }
        else
        {
            isClimbing = false; // No ladder detected, reset climbing state
        }

        if (isClimbing)
        {
            // Move the player along the ladder
            controller.Move(move * Time.deltaTime * currentSpeed);
        }
        else
        {
            controller.Move(move * Time.deltaTime * currentSpeed);

            // Changes the height position of the player..
            if (jumpControl.action.IsPressed() && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }

        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
