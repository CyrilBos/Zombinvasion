using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    private int curHealth = 100, maxHealth = 100;

    public int CurHealth { get { return curHealth; } set { curHealth = value; } }
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    public abstract void TakeDamage(int amount);

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
}
