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
        else
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, currentEnemy.transform.position - transform.position);
            if (Physics.Raycast(transform.position, currentEnemy.transform.position - transform.position, out hit))
            {
                transform.LookAt(currentEnemy.transform);
                anim.SetBool("attacking", true);
            }
            else
            {
                anim.SetBool("attacking", false);
            }
        }

    }


    public void AttackEnemy()
    {
        if (currentEnemy != null)
        {
            enemyController.TakeDamage(this.damage);
        }
    }

    public void TargetEnemy(GameObject enemy)
    {
        Debug.Log(enemy.tag);
        currentEnemy = enemy;
        enemyController = enemy.GetComponent<Character>();
    }

    protected override void OnDeath()
    {
        gameManager.ZombieDied(gameObject);
    }
}
