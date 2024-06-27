using System;
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
    
    [SerializeField] private Transform cameraFollowTarget; 
    [SerializeField] private float minCameraClamp; 
    [SerializeField] private float maxCameraClamp; 
    [SerializeField] private GameObject mainCam;
    
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float speedBoostMultiplier = 2.0f; 
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float crouch;
    
    //[SerializeField] private float rotationSpeed = 4f;
    
    

    private float climbingTimer;
    
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isClimbing;
    private bool isStartingToClimb;
    private Transform cameraMainTransform;
    private float xRotation;
    private float yRotation;
    //private Animator animator;
    

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
        /*cameraMainTransform = Camera.main.transform;*/
    }

    void Update()
    {
        UpdateGroundedStatus();
        HandleMovement();
        HandleClimbingTransition();
        print("Test");
    }

    private void LateUpdate()
    {
        CameraRotation();
    }


    private void UpdateGroundedStatus()
    {
        //print(transform.position.y);
        var ray = new Ray(transform.position, Vector3.down);
        groundedPlayer = Physics.Raycast(ray, 0.1f, groundLayer);

        
    }

    private void HandleMovement()
    {
        
        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        /*move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;*/
        float targetRotation = 0;
        move.y = 0;

        float currentSpeed = playerSpeed;
        if (speedBoostControl.action.IsPressed())
        {
            currentSpeed *= speedBoostMultiplier;
        }

        if (isClimbing)
        {
            HandleClimbing(movement, currentSpeed);
        }
        else
        {
            HandleWalkingAndJumping(move, currentSpeed);
        }

        
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
            //animator.SetBool("Climbing", false);
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

        float targetRotation;
        
        if (move != Vector3.zero && !isClimbing)
        {
            
            targetRotation = Quaternion.LookRotation(move).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            move = rotation * move;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
        }

        var localMove = move * currentSpeed * Time.deltaTime;
        controller.Move(localMove + playerVelocity * Time.deltaTime);
        
        //controller.Move(baseMove + playerVelocity * Time.deltaTime);
        print("Gravity");
    }

    /*private void RotatePlayer(Vector2 movement)
    {
        float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg /*+ cameraMainTransform.eulerAngles.y#1#;
        Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }*/
    

    private void CameraRotation()
    {
        Vector2 lookAround = lookControl.action.ReadValue<Vector2>();
        
        xRotation -= lookAround.y;
        yRotation += lookAround.x;
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
                    print("Start Climbing");
                    //animator.SetBool("Climbing", true);
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