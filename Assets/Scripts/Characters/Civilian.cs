using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : Character
{
    [SerializeField]
    private float fleeRadius = 10f, fleeDestinationDistance = 30f;

    private GameManager gameManager;
    private UnityEngine.AI.NavMeshAgent navAgent;

    private Animation anim;

    void Awake()
    {
        CurHealth = MaxHealth = 50;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, fleeRadius);
        bool foundDestination = false;
        int i = 0;
        while (!foundDestination && i < colliders.Length)
        {
            GameObject go = colliders[i].gameObject;
            string tag = go.tag;
            if (tag == "Building")
            {
                navAgent.SetDestination(go.transform.position);
                foundDestination = true;
            }
            else if (go.tag == "Player")
            {
                Vector3 direction = (go.transform.position - transform.position).normalized;
                navAgent.SetDestination(transform.position + direction * fleeDestinationDistance);
                foundDestination = true;
            }

            i++;
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
