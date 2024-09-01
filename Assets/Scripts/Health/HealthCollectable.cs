using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
[SerializeField]private float healthValue;
[Header("Sound Parameters")]
[SerializeField] private AudioClip life;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.tag == "Player")
    {
        collision.GetComponent<Health>().AddHealth(healthValue);
        gameObject.SetActive(false);
        SoundManager.instance.PlaySound(life);
    }
  }
}
