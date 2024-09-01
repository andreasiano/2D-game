using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed; 
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private Animator anim;
    private Vector3 initialScale;
    private BoxCollider2D boxCollider;
    private AudioSource audioSource;
    private float wallJumpCoolDown;
    private float horizontalInput;

    [Header("Sound Parameters")]
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip run;

    // Add a reference to the Health component
    private Health health;

    private void Awake()
    {
        // Grab references 
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        health = GetComponent<Health>(); // Initialize health reference

        // Store the initial scale
        initialScale = transform.localScale;

        // Adjust the drag to reduce sliding
        body.drag = 2; // You can tweak this value to get the desired effect
    }

    private void Update()
    {
        // Check if the player is dead
        if (health.currentHealth <= 0) return; // Prevent movement if dead

        horizontalInput = Input.GetAxis("Horizontal");

        // Flip player when moving left/right using the initial scale
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);

        // Set Animator parameters
        anim.SetBool("Run", horizontalInput != 0);

        // Check if the player is running and play/stop the run sound accordingly
        if (horizontalInput != 0 && IsGrounded() && !audioSource.isPlaying)
        {
            // Start playing the run sound if the player starts running
            audioSource.clip = run;
            audioSource.loop = true; // Loop the sound while running
            audioSource.Play();
        }
        else if ((horizontalInput == 0 || !IsGrounded()) && audioSource.isPlaying && audioSource.clip == run)
        {
            // Stop the run sound if the player stops running or jumps
            audioSource.Stop();
        }

        anim.SetBool("Grounded", IsGrounded());

        // Ensure immediate change in velocity based on input
        if (wallJumpCoolDown > 0.2f)
        {
            if (horizontalInput != 0)
            {
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            }
            else
            {
                // Reset horizontal velocity when no input is detected
                body.velocity = new Vector2(0, body.velocity.y);
            }

            if (OnWall() && !IsGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
                // Allow wall jump when on the wall and not grounded
                if (Input.GetKey(KeyCode.Space))
                {
                    Jump();
                }
            }
            else
            {
                body.gravityScale = 7;

                if (Input.GetKey(KeyCode.Space) && IsGrounded())
                {
                    Jump();
                }
            }
        }
        else
        {
            wallJumpCoolDown += Time.deltaTime;
        }
    }

    private void Jump()
    {
        // Stop the run sound when the player jumps
        if (audioSource.isPlaying && audioSource.clip == run)
        {
            audioSource.Stop();
        }

        if (IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
            SoundManager.instance.PlaySound(jump);  // Play jump sound when jumping from the ground
        }
        else if (OnWall() && !IsGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 15, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * initialScale.x, initialScale.y, initialScale.z);
            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 4, 12);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * initialScale.x, initialScale.y, initialScale.z);
            }

            wallJumpCoolDown = 0; // Reset cool down
            anim.SetTrigger("Jump");
            SoundManager.instance.PlaySound(jump);  // Play jump sound when performing a wall jump
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }
}

