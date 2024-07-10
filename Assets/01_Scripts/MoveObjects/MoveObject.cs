using UnityEngine;

[RequireComponent(typeof(LayerObject))]
public class MoveObject : MonoBehaviour
{
    public enum MovementDirection
    {
        LeftRight,
        ForwardBackward
    }

    public MovementDirection direction = MovementDirection.LeftRight;
    public float distance = 5f; // How far the object moves
    public float speed = 2f; // How fast the object moves

    public GameObject destinationTarget;

    private Vector3 endPosition;
    private Vector3 startPosition;
    private bool movingToEnd = true;
    [SerializeField] private bool isFrozen = false; // Flag to control movement

    public bool IsLocked { get => isFrozen; set => isFrozen = value; }

    void Start()
    {
        startPosition = transform.position;

        if (direction == MovementDirection.LeftRight)
        {
            endPosition = startPosition + Vector3.right * distance;
        }
        else if (direction == MovementDirection.ForwardBackward)
        {
            endPosition = startPosition + Vector3.forward * distance;
        }
        else if (destinationTarget != null)
        {
            endPosition = destinationTarget.gameObject.transform.position;
            endPosition.y = startPosition.y;
        }
    }

    public void UpdateTargetEndPosition()
    {
        if (destinationTarget != null)
            endPosition = destinationTarget.transform.position;
    }

    void Update()
    {
        if (!isFrozen)
        {
            Move();
        }
    }

    void Move()
    {
        if (movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, endPosition) < 0.01f)
            {
                movingToEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                movingToEnd = true;
            }
        }
    }

    public void ToggleFreeze()
    {
        isFrozen = !isFrozen;
    }
}
