using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attacker : Character
{
    private int damage = 25;
    public int Damage { get { return damage; } set { damage = value; } }
}
