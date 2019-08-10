using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 followOffset;

    [SerializeField]
    private float LerpSpeed = 0.1f;

    private GameObject target;
    public GameObject Target { get { return target; } set { target = value; } }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.transform.position + followOffset, LerpSpeed * Time.deltaTime);
    }
}
