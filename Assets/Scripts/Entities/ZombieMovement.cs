using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
public class ZombieMovement : MonoBehaviour
{
    [SerializeField]
    private float aggroRadius = 2f, triggerFollowDistance = 3f;
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
        else if (Vector3.Distance(transform.position, currentEnemy.transform.position) > triggerFollowDistance)
        {
            navAgent.SetDestination(currentEnemy.transform.position);
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRadius, LayerHelpers.NoGroundMask);
        if (colliders.Length > 0)
        {
            GameObject obj = colliders[0].gameObject;
            GameObject nearestEnemy = null;
            float nearestDistance = Mathf.Infinity;
            if (obj.tag == "Enemy" || obj.tag == "Building" || obj.tag == "Civilian")
            {
                nearestEnemy = colliders[0].gameObject;
                nearestDistance = Vector3.Distance(transform.position, colliders[0].gameObject.transform.position);
            }
            for (int i = 1; i < colliders.Length; i++)
            {
                obj = colliders[i].gameObject;
                if (obj.tag == "Enemy" || obj.tag == "Building" || obj.tag == "Civilian")
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

        return null;
    }
}
