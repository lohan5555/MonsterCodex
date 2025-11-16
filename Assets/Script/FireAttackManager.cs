using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UIElements;


[System.Serializable]
public class FireAttackType
{
    public string attackName;

    [Header("Type d'effet (un seul des deux)")]
    public GameObject effectPrefab;       // Ex: boule de feu (GameObject)
    public VisualEffect vfxPrefab;        // Ex: tornade, météorites, zone de feu

    [Header("Paramètres de l'attaque")]
    [Range(0f, 1f)] public float spawnProbability = 0.25f;
    public float duration = 3f;

    [Header("Points de spawn")]
    public List<Transform> spawnPoints = new List<Transform>(); // plusieurs positions possibles

    [Header("Multiplicité")]
    public int simultaneousSpawns = 1; // combien d'effets de ce type par attaque
}

public class FireAttackManager : MonoBehaviour
{

    [Header("Audio Settings")]
    public AudioSource audioSource;     // référence à l'AudioSource
    public AudioClip combatMusic;       // clip à jouer

    [Header("Fire Attack Settings")]
    public FireAttackType fireball;       // effet projectile
    public FireAttackType tornado;        // VFX secondaire
    public FireAttackType meteorRain;     // attaque rare
    public FireAttackType fireZone;       // zone de feu permanente

    [Header("Global Settings")]
    public float attackInterval = 3f;
    public bool combatActive = false;

    private bool isAttacking = false;
    private GameObject activeFireZoneInstance;
    public int maxHealth = 20;

    private VisualElement talkVE;   
    void Awake()
    {
        // Récupération du UIDocument et de ButtonTalk
        UIDocument doc = FindObjectOfType<UIDocument>();
        if (doc != null)
        {
            var root = doc.rootVisualElement;
            talkVE = root.Q<VisualElement>("ButtonTalk");
        }
    }


    void Update()
    {
        if (combatActive)
        {
            if (!isAttacking)
                StartCoroutine(AttackRoutine());

            if (activeFireZoneInstance == null && fireZone.vfxPrefab != null)
                SpawnFireZone();
        }
        else
        {
            if (activeFireZoneInstance != null)
                Destroy(activeFireZoneInstance);
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        FireAttackType selectedAttack = ChooseAttack();
        if (selectedAttack != null)
        {
            for (int i = 0; i < selectedAttack.simultaneousSpawns; i++)
                SpawnAttack(selectedAttack);
        }

        yield return new WaitForSeconds(attackInterval);
        isAttacking = false;
    }

    private void SpawnAttack(FireAttackType attack)
    {
        if (attack.spawnPoints.Count == 0)
        {
            Debug.LogWarning($"Aucun spawn point défini pour l'attaque {attack.attackName}");
            return;
        }

        Transform spawn = attack.spawnPoints[Random.Range(0, attack.spawnPoints.Count)];
        if (spawn == null) return;

        if (attack.effectPrefab != null)
        {
            GameObject go = Instantiate(attack.effectPrefab, spawn.position, spawn.rotation);
            // --- AJOUT AUTOMATIQUE DU DAMAGE ZONE ---
            if (go.GetComponent<DamageZone>() == null)
            {
                var dmg = go.AddComponent<DamageZone>();
                dmg.damagePerTick = 5;      //  choisis tes dégâts
                dmg.tickInterval = 0.5f;    // toutes les 0.5 sec
            }
            Destroy(go, attack.duration);
        }
        else if (attack.vfxPrefab != null)
        {
            VisualEffect vfx = Instantiate(attack.vfxPrefab, spawn.position, spawn.rotation);
            vfx.Play();
            // --- AJOUT AUTOMATIQUE DU DAMAGE ZONE ---
            if (vfx.GetComponent<DamageZone>() == null)
            {
                var dmg = vfx.gameObject.AddComponent<DamageZone>();
                dmg.damagePerTick = 5;      //  choisis tes dégâts
                dmg.tickInterval = 0.5f;
            }
            Destroy(vfx.gameObject, attack.duration);
        }
    }

    private FireAttackType ChooseAttack()
    {
        float roll = Random.value;
        float cumulative = 0f;

        FireAttackType[] attacks = { fireball, tornado, meteorRain };
        foreach (var atk in attacks)
        {
            cumulative += atk.spawnProbability;
            if (roll <= cumulative)
                return atk;
        }

        return fireball;
    }

    private void SpawnFireZone()
    {
        if (fireZone.spawnPoints.Count == 0)
        {
            Debug.LogWarning("Aucun spawn point défini pour la zone de feu !");
            return;
        }

        Transform spawn = fireZone.spawnPoints[0];
        VisualEffect vfx = Instantiate(fireZone.vfxPrefab, spawn.position, spawn.rotation);
        vfx.Play();
        activeFireZoneInstance = vfx.gameObject;
    }

    public void StartFireCombat()
    {
        combatActive = true;
        
        BossUI.Instance.ShowBossBar("Gandalf", maxHealth);
        MusicManager.Instance.PlayBossMusic();

    }

    public void StopFireCombat()
    {
        combatActive = false;

        if (activeFireZoneInstance != null)
        {
            Destroy(activeFireZoneInstance);
            activeFireZoneInstance = null;
        }

        MusicManager.Instance.StopBossMusicAndResumeLevel();
        if (talkVE != null)
            talkVE.style.display = DisplayStyle.None;
    }

}
