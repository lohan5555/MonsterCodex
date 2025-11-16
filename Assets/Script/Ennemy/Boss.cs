using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Stats du Boss")]
    public int maxHealth = 20;
    private int currentHealth;

    [Header("Loot & Effets")]
    public GameObject lootPrefab;        // L'objet à drop
    public GameObject deathEffectPrefab; // Fumée
    public float despawnDelay = 0.2f;    // Petit délai (ou 0)

    [Header("Fire Attack Manager")]
    public FireAttackManager fireAttackManager; // assigner depuis l'inspecteur

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("[Boss] Max Boss HP : " + maxHealth);

    }

    public void TakeDamage(int amount)
    {
        BossUI.Instance.UpdateBossHealth(currentHealth, maxHealth);
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("[Boss] Boss HP : " + currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("[Boss] Boss mort !");
        BossUI.Instance.HideBossBar();



        // Stoppe toutes les attaques de feu
        if (fireAttackManager != null)
        {
            fireAttackManager.StopFireCombat();
        }

        // Effet visuel de fumée
        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        // Drop de l'objet
        if (lootPrefab != null)
            Instantiate(lootPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        // Destruction du boss après un léger délai
        Destroy(gameObject, despawnDelay);
    }
}
