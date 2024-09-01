using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Animator")]
    [SerializeField] private Animator anim;

    // Add a reference to the Health component
    private Health health;

    private void Awake()
    {
        initScale = enemy.localScale;
        health = GetComponent<Health>(); // Initialize health reference
    }

    private void OnDisable()
    {
        anim.SetBool("Moving", false);
    }

    private void Update()
    {
        // Check if the enemy is dead
        if (health.currentHealth <= 0) 
        {
            anim.SetBool("Moving", false); // Ensure the enemy stops animating
            return; // Prevent movement if dead
        }

        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("Moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("Moving", true);
        // Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        // Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }

    public void StopMovement()
    {
        // Call this method to stop the enemy from moving
        movingLeft = false; // Stop moving
        anim.SetBool("Moving", false); // Stop any movement animations
    }
}


