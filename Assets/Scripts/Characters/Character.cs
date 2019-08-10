using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private int curHealth = 100, maxHealth = 100;
    private int damage = 25;
    public int Damage { get { return damage; } }

    public Character() { }

    public Character(int health, int damage)
    {
        this.curHealth = health;
        this.maxHealth = health;
        this.damage = damage;
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
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
