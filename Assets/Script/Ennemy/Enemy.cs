using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 2;
    public Animator animator;
    public EnemyHitbox weaponHitbox;
    public float invulnerabilityTime = 0.5f;
    public float attackTime = 5f;
    private bool isInvulnerable = false;
    private bool isAttacking = false;
    public Transform player;                 // Référence vers le joueur
    public float speed = 2f;                 // Vitesse de déplacement
    public float gravity = -9.81f;           // Gravité
    public float stopDistance = 1.5f;        // Distance minimale avant de s'arrêter
    public float detectionDistance = 3f;     // Distance de détection du joueur
    public Transform groundCheck;            // Le sol
    private bool isGrounded;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    public float knockbackDistance = 2f;
    public float knockbackSpeed = 5f;
    private bool alive = true;
    public GameObject loot;
    public GameObject smokeEffect;
    public float despawnTime = 3f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (player == null) //si on ne le trouve pas on le cherche
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        //vérif pour être sur qu'on touche le sol
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Si l’ennemi est au sol et qu’il tombe encore, on l’arrête et on le garde légèrement poussé vers le bas pour rester bien ancré.
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        // Distance joueur - ennemi
        float distance = Vector3.Distance(transform.position, player.position);

        // Déplacement horizontal (X et Z)
        Vector3 move = Vector3.zero;
        
        Vector3 direction = player.position - transform.position;
        if (direction != Vector3.zero && distance < detectionDistance && alive)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                0.1f
            );

        if (distance > stopDistance && distance < detectionDistance)
        {
            animator.SetBool("Follow", true);

            direction.y = 0; // on ignore la différence de hauteur
            direction.Normalize();

            move = direction * speed;

            // Tourner vers le joueur

        }
        else if (distance > detectionDistance)
        {
            animator.SetBool("Follow", false);
        }
        else if (!isAttacking && health > 1)
        {
            AttackPlayer("Attack1");
        }
        else if (!isAttacking && health <= 1)
        {
            AttackPlayer("Attack2");
        }

        if (alive)
        {
            // Application du mouvement horizontal (X et Z)
            Vector3 horizontalMove = new Vector3(move.x, 0, move.z);
            controller.Move(horizontalMove * Time.deltaTime);   
        }

        // Application du mouvement vertical (Y)
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }

    public void AttackPlayer(string attack)
    {
        Debug.Log("Animation d'attaque : " + attack);
        StartCoroutine(WaitAttackCooldown());
        animator.SetTrigger(attack);
    }

    public void TakeDamage(int damage, Vector3 attackerPosition)
    {
        if (isInvulnerable) return;

        health -= damage;
        Debug.Log("Enemy took damage, health = " + health);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit");

            Vector3 knockbackDir = (transform.position - attackerPosition).normalized;
            knockbackDir.y = 0f; //on ignore l'axe Y pour ne pas enfoncer l'Enemy dans le sol
            knockbackDir = knockbackDir.normalized;
            StartCoroutine(DoKnockback(knockbackDir));
        }

        StartCoroutine(InvulnerabilityCooldown());
    }

    private IEnumerator DoKnockback(Vector3 direction)
    {
        float distancePushed = 0f;

        while (distancePushed < knockbackDistance)
        {
            float step = knockbackSpeed * Time.deltaTime;
            
            // Move via CharacterController (collision + pente gérés automatiquement)
            controller.Move(direction * step);

            distancePushed += step;
            yield return null;
        }
    }


    private IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    private IEnumerator WaitAttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
    }

    public void Die()
    {
        alive = false;
        animator.SetTrigger("Die");
        weaponHitbox.DisableHitbox();
        StartCoroutine(WaitDespawn());
    }
    private IEnumerator WaitDespawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
        if (smokeEffect != null)
        {
            Instantiate(smokeEffect, transform.position, Quaternion.identity);
        }
        if (loot != null)
        {
            //on fait apparaitre le loot légèrement au dessus de la position de l'Enemy
            Instantiate(loot, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }
}
