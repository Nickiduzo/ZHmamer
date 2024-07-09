using UnityEngine;
using System.Collections;

public class EnemyKnife : MonoBehaviour
{
    [SerializeField] private int damageToPlayer = 20;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] protected Animator anim;

    protected Transform player;
    protected bool canAttack = true;
    private Collider2D attackCollider;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        CheckForPlayerInAttackRange();
    }

    private void CheckForPlayerInAttackRange()
    {
        if (player == null) return;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackCollider.bounds.center, attackCollider.bounds.size, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") && canAttack)
            {
                AttackPlayer();
                break;
            }
        }
    }

    protected virtual void AttackPlayer()
    {
        anim.SetTrigger("isAttacking");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.DecreaseHealth(damageToPlayer);
        }
        StartCoroutine(AttackCooldown());
    }

    protected virtual IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        if (attackCollider == null)
            attackCollider = GetComponent<Collider2D>();

        if (attackCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackCollider.bounds.center, attackCollider.bounds.size);
        }
    }
}
