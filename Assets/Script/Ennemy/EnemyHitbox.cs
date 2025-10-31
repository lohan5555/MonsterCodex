using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 1;
    private Collider hitbox;

    void Awake()
    {
        hitbox = GetComponent<Collider>();
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Touched " + other);
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
