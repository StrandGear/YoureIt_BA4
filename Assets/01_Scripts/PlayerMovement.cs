using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;
    [SerializeField] private InputActionReference speedBoostControl; 
    [SerializeField] private InputActionReference interactControl; 
    [SerializeField] private InputActionReference lookControl;
    [SerializeField] private InputActionReference crouchControl;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animControl;
    
    [SerializeField] private Transform cameraFollowTarget; 
    [SerializeField] private float minCameraClamp; 
    [SerializeField] private float maxCameraClamp; 
    [SerializeField] private GameObject mainCam;
    
    
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float speedBoostMultiplier = 2.0f; 
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    
    [SerializeField] private float crouchSpeed;
    
    
    private float climbingTimer;
    
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isClimbing;
    private bool isStartingToClimb;
    private bool isCrouching;
    private bool canStand;
    private Transform cameraMainTransform;
    private float xRotation;
    private float yRotation;
    
    private Vector3 originalCenter;
    private float originalHeight;
    private float originalRadius;
    //private Animator animator;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        animControl = gameObject.GetComponent<Animator>();
        
        originalCenter = controller.center;
        originalHeight = controller.height;
        originalRadius = controller.radius;
    }

    void Update()
    {
        UpdateGroundedStatus();
        HandleMovement();
        HandleCrouching(); // Call HandleCrouching in Update
        HandleClimbingTransition();
        
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void UpdateGroundedStatus()
    {
        var ray = new Ray(transform.position, Vector3.down);
        groundedPlayer = Physics.Raycast(ray, 0.1f, groundLayer);
    }

    private void HandleMovement()
    {
        Vector2 movement = movementControl.action.ReadValue<Vector2>();

        // Calculate the movement direction relative to the camera
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        Vector3 forward = cameraMainTransform.forward;
        Vector3 right = cameraMainTransform.right;

        // Normalize vectors to ensure consistent movement speed in all directions
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
       

        // Calculate final movement vector
        Vector3 desiredMoveDirection = forward * move.z + right * move.x;

        float currentSpeed = playerSpeed;
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        if (speedBoostControl.action.IsPressed() && !isCrouching)
        {
            currentSpeed *= speedBoostMultiplier;
        }

        if (isClimbing)
        {
            HandleClimbing(movement, currentSpeed);
        }
        else
        {
            HandleWalkingAndJumping(desiredMoveDirection, currentSpeed);
        }
        
        // Calculate forwards and sideways speeds
        float forwardSpeed = Vector3.Dot(desiredMoveDirection.normalized, forward);
        float sidewaysSpeed = Vector3.Dot(desiredMoveDirection.normalized, right);

        // Update animator parameters
        animControl.SetFloat("ForwardsSpeed", forwardSpeed);
        animControl.SetFloat("SidewaysSpeed", sidewaysSpeed);

        UpdateClimbingTimer();
    }

    private void HandleClimbing(Vector2 movement, float currentSpeed)
    {
        Vector3 move = new Vector3(0, movement.y, 0);
        controller.Move(move * Time.deltaTime * currentSpeed);
        Debug.Log("Climbing");

        float ladderGrabDistance = 1f;
        Vector3 raycastStart = transform.position + Vector3.up * 0.1f;

        if (!Physics.Raycast(raycastStart, transform.forward, out RaycastHit raycastHit, ladderGrabDistance) ||
            !raycastHit.transform.CompareTag("Ladder") || (groundedPlayer && !isStartingToClimb))
        {
            isClimbing = false;
        }
    }

    private void HandleWalkingAndJumping(Vector3 move, float currentSpeed)
    {
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            Debug.Log("Player Jumped");
        }

        if (groundedPlayer && playerVelocity.y <= 0)
        {
            playerVelocity.y = 0f;
        }
        else
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }

        if (move != Vector3.zero && !isClimbing)
        {
            float targetRotation = Quaternion.LookRotation(move).eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
        }

        var localMove = move * currentSpeed * Time.deltaTime;
        controller.Move(localMove + playerVelocity * Time.deltaTime);
    }

    private void HandleCrouching()
    {
        if (crouchControl.action.triggered)
        {
            if (isCrouching)
            {
                isCrouching = false;
                jumpHeight = 1.0f;
                controller.center = originalCenter;
                controller.height = originalHeight;
                controller.radius = originalRadius;
            }
            else
            {
                isCrouching = true;
                jumpHeight = 0f;
                controller.height = 0.5f;
                controller.center = new Vector3(0f, 0.5f, 0f);
                controller.radius = 0.5f;

            }
        }
    }

    private void CameraRotation()
    {
        Vector2 lookAround = lookControl.action.ReadValue<Vector2>();

        xRotation -= lookAround.y * Time.deltaTime * 10;
        yRotation += lookAround.x * Time.deltaTime * 10;
        xRotation = Mathf.Clamp(xRotation, minCameraClamp, maxCameraClamp);
        Quaternion camRotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollowTarget.rotation = camRotation;
    }

    private void UpdateClimbingTimer()
    {
        if (isStartingToClimb)
        {
            climbingTimer -= Time.deltaTime;
            if (climbingTimer < 0)
            {
                isStartingToClimb = false;
            }
        }
    }

    private void HandleClimbingTransition()
    {
        if (interactControl.action.triggered && !isClimbing)
        {
            Vector3 raycastStart = transform.position + Vector3.up * 0.1f;
            float ladderGrabDistance = 1f;

            if (Physics.Raycast(raycastStart, transform.forward, out RaycastHit raycastHit, ladderGrabDistance))
            {
                if (raycastHit.transform.CompareTag("Ladder"))
                {
                    isClimbing = true;
                    groundedPlayer = true;
                    isStartingToClimb = true;
                    climbingTimer = 2f;
                    playerVelocity.y = 0f;
                }
            }
        }
    }

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        speedBoostControl.action.Enable();
        interactControl.action.Enable();
        lookControl.action.Enable();
        crouchControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        speedBoostControl.action.Disable();
        interactControl.action.Disable();
        lookControl.action.Disable();
        crouchControl.action.Disable();
    }
}