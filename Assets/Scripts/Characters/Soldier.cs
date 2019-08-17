using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Attacker
{
    public float attackRange = 6f;
    public float AttackCooldownValue = 2f;

    public GameObject muzzleFlashGO;

    private float attackCooldown;

    private GameManager gameManager;
    private UnityEngine.AI.NavMeshAgent navAgent;
    private Animator anim;
    private ParticleSystem muzzleFlash;
    private AudioSource gunSound;

    private GameObject target;
    private ZombieAttack targetController;

    void Awake()
    {
        Damage = 15;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        muzzleFlash = muzzleFlashGO.GetComponent<ParticleSystem>();
        gunSound = GetComponent<AudioSource>();
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
            StopShooting();
        }
        else if (Vector3.Distance(target.transform.position, transform.position) > attackRange)
        {
            navAgent.SetDestination(target.transform.position);
        }
        else
        {
            if (navAgent.hasPath)
            {
                navAgent.ResetPath();
            }
            transform.LookAt(target.transform);
            if (attackCooldown <= 0f)
            {
                anim.SetBool("shooting", true);
                muzzleFlash.Play();
                gunSound.Play();

                targetController.TakeDamage(this.Damage);
                if (targetController.IsDead())
                {
                    target = null;
                    targetController = null;
                    StopShooting();
                }
                attackCooldown = AttackCooldownValue;
            }
        }

        anim.SetFloat("speed", navAgent.velocity.sqrMagnitude);
    }

    private void StopShooting()
    {
        anim.SetBool("shooting", false);
        muzzleFlash.Stop();
        gunSound.Stop();
    }

    public override void TakeDamage(int amount)
    {
        LooseHealth(amount);
        if (IsDead())
        {
            gameManager.SpawnZombie(transform.position);
            Destroy(gameObject);
        }
    }
}
