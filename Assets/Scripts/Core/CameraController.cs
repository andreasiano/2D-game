using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float followSpeed = 0.1f; // Speed for following the player
    [SerializeField] private float transitionSpeed = 0.1f; // Speed for room transitions
    private float targetX; // Target X position for room transitions
    private Vector3 velocity = Vector3.zero; // Velocity for SmoothDamp
    private bool isMovingToRoom = false; // Indicates if the camera is moving to a new room
    private Transform player; // Reference to the player

    private void Start()
    {
        // Find the player transform on startup
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetX = player.position.x; // Initialize target position to player's current position
    }

    private void Update()
    {
        // If moving to a new room, transition the camera to the target position
        if (isMovingToRoom)
        {
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, transitionSpeed);

            // Check if the camera has reached the target position
            if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
            {
                isMovingToRoom = false; // Stop moving to a new room when the target is reached
            }
        }
        else
        {
            // Smoothly follow the player
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
        }
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        targetX = _newRoom.position.x; // Set the target position to the new room's x position
        isMovingToRoom = true; // Start moving to the new room
    }
}





