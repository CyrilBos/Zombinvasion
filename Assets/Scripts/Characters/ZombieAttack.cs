using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : Attacker
{
    private int zombIndex;
    public int ZombIndex { get { return zombIndex; } set { zombIndex = value; } }

    [SerializeField]
    private float attackRange = 1f;

    private GameManager gameManager;

    private GameObject currentEnemy;
    private Character enemyController;

    private Animator anim;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemy == null)
        {
            anim.SetBool("attacking", false);
        }
        else if (enemyController.IsDead())
        {
            currentEnemy = null;
            enemyController = null;
            anim.SetBool("attacking", false);
        }
        else if (Vector3.Distance(currentEnemy.transform.position, transform.position) < attackRange)
        {
            anim.SetBool("attacking", true);
        }
        else
        {
            anim.SetBool("attacking", false);
        }
    }


    public void AttackEnemy()
    {
        if (currentEnemy != null)
        {
            enemyController.TakeDamage(this.Damage);
        }
    }

    public void TargetEnemy(GameObject enemy)
    {
        currentEnemy = enemy;

        enemyController = enemy.GetComponent<Character>();
    }

    public override void TakeDamage(int amount)
    {
        this.LooseHealth(amount);
        if (this.IsDead())
        {
            gameManager.ZombieDied(gameObject);
            Destroy(gameObject);
        }
    }
}
