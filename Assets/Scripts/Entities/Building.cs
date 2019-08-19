using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Character
{
    [SerializeField]
    private int capacity = 5;

    private List<GameObject> refugedCivilians = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (IsNotFull() && other.gameObject.tag == "Civilian")
        {
            refugedCivilians.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnDeath()
    {
        foreach (GameObject civilian in refugedCivilians)
        {
            civilian.SetActive(true);
        }
    }

    public bool IsNotFull()
    {
        return refugedCivilians.Count < capacity;
    }
}
