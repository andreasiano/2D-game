using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRender;

    [Header("Sound Parameters")]
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip hurt;

    private PlayerRespawn playerRespawn;
    private UIManager uiManager; // Reference to UIManager

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        playerRespawn = GetComponent<PlayerRespawn>();
        uiManager = FindObjectOfType<UIManager>(); // Initialize UIManager reference
    }

    public void TakeDamage(float _damage)
    {
        if (dead || spriteRender.color.a < 1f) return; // Prevent damage if dead or invulnerable

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invulnerability());
            SoundManager.instance.PlaySound(hurt);
        }
        else
        {
            if (!dead)
            {
                Die(); // Call Die if health is zero
            }
        }
    }

    private void Die()
{
    dead = true;

    // Trigger death animation and sound
    anim.SetTrigger("Die");
    SoundManager.instance.PlaySound(death);
    
    // Check if this instance is the player
    if (gameObject.CompareTag("Player")) 
    {
        Debug.Log("Player died, showing Game Over screen.");
        uiManager.GameOver(); // Show Game Over screen
    }
    else
    {
        // Handle enemy death (e.g., play death animation, drop loot, etc.)
        Debug.Log("Enemy died, no Game Over screen shown.");
    }
}


    private IEnumerator HandleRespawn()
    {
        yield return new WaitForSeconds(2.0f); // Adjust delay to match the death animation time

        playerRespawn.Respawn();

        // Re-enable components
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    public void Respawn()
    {
        dead = false;
        currentHealth = startingHealth;
        anim.ResetTrigger("Die");
        anim.Play("Idle");

        // Re-enable collider and physics
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;

        StartCoroutine(Invulnerability());
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRender.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRender.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}












