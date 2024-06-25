using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;
    [SerializeField] private InputActionReference speedBoostControl; 
    [SerializeField] private InputActionReference interactControl; 
    [SerializeField] private LayerMask groundLayer; 
    
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float speedBoostMultiplier = 2.0f; 
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;

    private float climbingTimer;
    
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isClimbing;
    private bool isStartingToClimb;
    private Transform cameraMainTransform;
    //private Animator animator;
    

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
        cameraMainTransform = Camera.main.transform;
    }

    void Update()
    {
        
        var ray = new Ray(transform.position, Vector3.down);

        groundedPlayer = Physics.Raycast(ray, 1f, groundLayer);

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
        float ladderGrabDistance = 1f;
        Vector3 raycastStart = transform.position + Vector3.up * avoidFloorDistance;

        if (interactControl.action.triggered && !isClimbing)
        {
            if (Physics.Raycast(raycastStart, transform.forward, out RaycastHit raycastHit, ladderGrabDistance))
            {
                if (raycastHit.transform.CompareTag("Ladder")) 
                {
                    isClimbing = true;
                    groundedPlayer = true;
                    isStartingToClimb = true;// Ensure grounded for climbing
                    climbingTimer = 2f;
                    playerVelocity.y = 0f; // Reset vertical velocity
                    //animator.SetBool("Climbing", true);
                    
                }
            }
        }
        
        if (isClimbing)
        {
            // Move the player along the ladder
            move.x = 0f;
            move.z = 0f;
            move.y = movement.y; // Use the y-axis for climbing
            controller.Move(move * Time.deltaTime * currentSpeed);
            Debug.Log("Climbing");
            
            // Exit climbing if no vertical input or interact control is released
            if (!Physics.Raycast(raycastStart, transform.forward, out RaycastHit raycastHit, ladderGrabDistance) ||
                !raycastHit.transform.CompareTag("Ladder"))
            {
                isClimbing = false;
                
                //animator.SetBool("Climbing", false);
            }

            if (groundedPlayer &&!isStartingToClimb)
            {
                isClimbing = false;
            }
        }
        else
        {
            var baseMove = move * Time.deltaTime * currentSpeed;

            if (jumpControl.action.triggered && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                Debug.Log("Player Jumped");
            }
            

            playerVelocity.y += gravityValue * Time.deltaTime;
            
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            controller.Move(baseMove + playerVelocity * Time.deltaTime);
        }

        if (movement != Vector2.zero && !isClimbing)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            print("Rotate");
        }

        if (isStartingToClimb)
        {
            climbingTimer -= Time.deltaTime;
            if (climbingTimer < 0)
            {
                isStartingToClimb = false;
            }
        }
        
        float speed = move.magnitude;
        //animator.SetFloat("Speed", speed);
        
    }
    
    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        speedBoostControl.action.Enable(); 
        interactControl.action.Enable(); 
    }
    
    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        speedBoostControl.action.Disable(); 
        interactControl.action.Disable(); 
    }
}
