using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private int zombIndex;
    public int ZombIndex { get { return zombIndex; }  set {zombIndex = value; } }

    [SerializeField]
    private float attackRange = 1f;

    [SerializeField]
    private float attackSpeed = 1f;
    private float attackCooldown;

    private GameManager gameManager;

    public Character character = new Character();

    private GameObject currentEnemy;
    private EnemyController enemyController;

    private Animator anim;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        attackCooldown = attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
        if (currentEnemy == null)
        {
            anim.SetBool("attacking", false);
        }
        else if (Vector3.Distance(currentEnemy.transform.position, transform.position) < attackRange)
        {
            anim.SetBool("attacking", true);
            if (attackCooldown <= 0f)
            {
                if (enemyController.IsDead())
                {
                    currentEnemy = null;
                    enemyController = null;
                    anim.SetBool("attacking", false);
                } else
                {
                    enemyController.TakeDamage(character.Damage);
                }
                attackCooldown = attackSpeed;
            }
        }
        
    }

    public void TargetEnemy(GameObject enemy)
    {
        currentEnemy = enemy;
        enemyController = enemy.GetComponent<EnemyController>();
    }

    public void TakeDamage(int amount)
    {
        character.TakeDamage(amount);
        anim.Play("hit");
        if (character.IsDead())
        {
            gameManager.ZombieDied(gameObject);
            Destroy(gameObject);
        }
    }

    public bool IsDead()
    {
        return character.IsDead();
    }
}
