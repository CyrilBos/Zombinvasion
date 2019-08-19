using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth = 100, curHealth = 100;

    public void TakeDamage(int amount)
    {
        this.LooseHealth(amount);
        if (this.IsDead())
        {
            OnDeath();
            Destroy(gameObject);
        }
    }

    public void LooseHealth(int amount)
    {
        curHealth -= amount;
        if (curHealth < 0)
        {
            curHealth = 0;
        }
    }

    public bool IsDead()
    {
        return curHealth <= 0;
    }

    protected abstract void OnDeath();
}
