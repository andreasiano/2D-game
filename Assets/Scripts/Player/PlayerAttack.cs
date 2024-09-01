using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCoolDown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Sound Parameters")]
    [SerializeField] private AudioClip slash;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float coolDownTimer = Mathf.Infinity;

    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;

    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<Health>(); // Get the Health component
    }

   private void Update()
{
    // Ensure the player is alive before allowing attacks
    if (playerHealth.currentHealth > 0 && 
        Input.GetKeyDown(KeyCode.W) && 
        coolDownTimer > attackCoolDown && playerMovement.CanAttack())
    {
        Attack();
    }

    coolDownTimer += Time.deltaTime;
}


    private void Attack()
    {
        SoundManager.instance.PlaySound(slash);
        anim.SetTrigger("Attack");
        coolDownTimer = 0;
        DamageEnemy();
    }

    private void DamageEnemy()
    {
        // Calculate the center and size of the detection box
        Vector2 detectionCenter = (Vector2)transform.position + (Vector2.right * range * transform.localScale.x * colliderDistance);
        Vector2 boxSize = new Vector2(boxCollider.size.x * range, boxCollider.size.y);

        // Perform BoxCast to detect enemies
        RaycastHit2D hit = Physics2D.BoxCast(detectionCenter, boxSize, 0, Vector2.left, 0, enemyLayer);

        if (hit.collider != null)
        {
            Health enemyHealth = hit.collider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Damaged Enemy");
            }
        }
    }

  private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                             new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}



