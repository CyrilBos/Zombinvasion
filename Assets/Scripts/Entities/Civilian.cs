using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : Character
{
    [SerializeField]
    private float fleeRadius = 10f, fleeDestinationDistance = 30f;

    [SerializeField]
    static private float maxExplorationDistance = 10f;

    private GameManager gameManager;
    private UnityEngine.AI.NavMeshAgent navAgent;

    private Animation anim;

    private bool hasDestination = false;
    private Building building = null;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, fleeRadius);
        int i = 0;
        while (!hasDestination && i < colliders.Length)
        {
            GameObject go = colliders[i].gameObject;
            string tag = go.tag;
            if (tag == "Building")
            {
                Building foundBuilding = go.GetComponent<Building>();
                if (foundBuilding.IsNotFull())
                {
                    navAgent.SetDestination(go.transform.position);
                    building = foundBuilding;
                    hasDestination = true;
                }
            }
            else if (go.tag == "Player")
            {
                Vector3 direction = (go.transform.position - transform.position).normalized;
                navAgent.SetDestination(transform.position + direction * fleeDestinationDistance);
                hasDestination = true;
            }

            i++;
        }

        if (hasDestination && building != null && !building.IsNotFull())
        {
            hasDestination = false;
            building = null;
        }

        if (!hasDestination)
        {
            navAgent.SetDestination(transform.position + maxExplorationDistance * Random.insideUnitSphere);
        }

        if (navAgent.velocity.sqrMagnitude > 0)
        {
            anim.Play();
        }
        else
        {
            anim.Stop();
        }
    }

    protected override void OnDeath()
    {
        gameManager.SpawnZombie(transform.position);
    }
}
