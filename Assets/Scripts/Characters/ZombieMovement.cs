using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour {
    [SerializeField]
    private float aggroRadius = 2f;
    private GameObject currentEnemy;

    private ZombieAttack playerAttack;
    private UnityEngine.AI.NavMeshAgent navAgent;
    private Animator anim;
    private Vector3 destination;

    void Awake()
    {
        playerAttack = GetComponent<ZombieAttack>();
        anim = GetComponent<Animator>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemy == null)
        {
            currentEnemy = LookForNearestEnemy();

            if (currentEnemy != null)
            {
                playerAttack.TargetEnemy(currentEnemy);
                navAgent.SetDestination(currentEnemy.transform.position);
            }
        }

        anim.SetFloat("speed", navAgent.velocity.sqrMagnitude); 
    }

    public void SetNewDestination(Vector3 destination)
    {
        this.destination = destination;
        currentEnemy = null;
        anim.SetBool("attacking", false);
        navAgent.SetDestination(this.destination);
    }

    private GameObject LookForNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRadius);
            GameObject nearestEnemy = null;
            float nearestDistance = aggroRadius;
            foreach (Collider collider in colliders)
            {
                GameObject obj = collider.gameObject;
                if (obj.tag == "Enemy")
                {
                    float distance = Vector3.Distance(obj.transform.position, transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestEnemy = obj;
                        nearestDistance = distance;
                    }
                }
            }
        return nearestEnemy;
    }
}
