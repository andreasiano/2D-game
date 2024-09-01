using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound; // Sound to play when activating a checkpoint
    private Transform currentCheckpoint; // Current checkpoint the player has reached
    private Health playerHealth; // Reference to the player's health component
    private UIManager uiManager; // Reference to UIManager

    // Store the original scale
    private Vector3 originalScale;

    private void Awake()
    {
        playerHealth = GetComponent<Health>(); // Initialize the health component reference
        uiManager = FindObjectOfType<UIManager>(); // Initialize UIManager reference
        originalScale = transform.localScale; // Store the original scale
    }

    public void Respawn()
    {
        if (currentCheckpoint == null)
        {
            Debug.Log("No checkpoint set, showing Game Over screen.");
            // Show game over screen
            uiManager.GameOver();
            return; // Don't execute the rest of this function
        }

        // Move the player to the current checkpoint position
        transform.position = currentCheckpoint.position;

        // Restore player health and reset animation
        playerHealth.Respawn();

        // Set player facing direction to the right without affecting size
        SetFacingDirection();

        // Move camera to checkpoint room
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController != null)
        {
            Debug.Log("Moving camera to checkpoint room: " + currentCheckpoint.parent.name);
            cameraController.MoveToNewRoom(currentCheckpoint.parent);
        }
        else
        {
            Debug.LogError("CameraController not found on the main camera.");
        }
    }

    // Activate checkpoint when player collides with it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform; // Set the current checkpoint to the collided checkpoint
            Debug.Log("Checkpoint set at: " + currentCheckpoint.position);

            SoundManager.instance.PlaySound(checkpointSound); // Play checkpoint sound
            collision.GetComponent<Collider2D>().enabled = false; // Disable the checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("Appear"); // Trigger any animation associated with the checkpoint
        }
    }

    // Method to set the player's facing direction to the right
    private void SetFacingDirection()
    {
        // Set local scale to original scale and face right
        transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Ensure the player is facing right
    }
}





















