using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float attackRange = 6f;
    public float AttackCooldownValue = 2f;

    private float attackCooldown;

    public Character character = new Character(100, 10);

    private GameManager gameManager;
    private UnityEngine.AI.NavMeshAgent navAgent;
    private Animator anim;

    private GameObject target;
    private ZombieAttack targetController;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        attackCooldown = AttackCooldownValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (target == null)
        {
            target = gameManager.GetNearestZombie(transform.position);
            if (target)
            {
                targetController = target.GetComponent<ZombieAttack>();
            }
            anim.SetBool("shooting", false);
        }
        else if (Vector3.Distance(target.transform.position, transform.position) > attackRange) {
            navAgent.SetDestination(target.transform.position);
        }
        else {
            if (navAgent.hasPath)
            {
                navAgent.ResetPath();
            }
            if (attackCooldown <= 0f)
            {
                anim.SetBool("shooting", true);
                targetController.TakeDamage(character.Damage);
                if (targetController.IsDead())
                {
                    target = null;
                    targetController = null;
                    anim.SetBool("shooting", false);
                }
                attackCooldown = AttackCooldownValue;
            }
        }

        anim.SetFloat("speed", navAgent.velocity.sqrMagnitude);
    }

    public void TakeDamage(int amount)
    {
        character.TakeDamage(amount);
        if (character.IsDead())
        {
            gameManager.SpawnZombie(transform.position);
            Destroy(gameObject);
        }
    }

    public bool IsDead()
    {
        return character.IsDead();
    }
}
