using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Character
{
    [SerializeField]
    private int capacity = 5;

    private List<GameObject> refugedCivilians = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (refugedCivilians.Count < capacity && other.gameObject.tag == "Civilian")
        {
            Debug.Log("civilian");
            refugedCivilians.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    public override void TakeDamage(int amount)
    {
        LooseHealth(amount);
        if (IsDead())
        {
            foreach (GameObject civilian in refugedCivilians)
            {
                civilian.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
