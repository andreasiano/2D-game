using UnityEngine;

public class EnemyAttack : MonoBehaviour
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

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    // References
    private float coolDownTimer = Mathf.Infinity;
    private Animator anim;
    private Health playerHealth; // Reference to player health component
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        coolDownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (playerHealth != null && playerHealth.currentHealth > 0) // Check if player is alive
            {
                if (coolDownTimer >= attackCoolDown)
                {
                    coolDownTimer = 0;
                    anim.SetTrigger("EnemyAttack");
                }
            }
        }

        // Attack only when the player is in sight
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                                             new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
                                             0, Vector2.left, 0, playerLayer);
        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                             new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            // Only damage player if they are alive
            if (playerHealth != null && playerHealth.currentHealth > 0)
            {
                playerHealth.TakeDamage(damage);
                SoundManager.instance.PlaySound(slash);
            }
        }
    }
}





