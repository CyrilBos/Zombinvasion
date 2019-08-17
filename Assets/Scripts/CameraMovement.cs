using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 followOffset;

    [SerializeField]
    private float LerpSpeed = 0.1f;

    [SerializeField]
    private float shakeMinIntensity = 0.1f, shakeMaxIntensity = 0.2f, shakeMinDuration = 0.5f, shakeMaxDuration = 1f;

    private GameObject target;
    public GameObject Target { get { return target; } set { target = value; } }

    private float shakeDuration = 1f;
    private float shakeIntensity = 0f;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.transform.position + followOffset, LerpSpeed * Time.deltaTime);

        if (shakeDuration > 0f)
        {
            transform.localPosition += Random.insideUnitSphere * shakeIntensity;

            shakeDuration -= Time.deltaTime;
        }
    }

    public void ScreenShake()
    {
        shakeDuration = Random.Range(shakeMinDuration, shakeMaxDuration);
        shakeIntensity = Random.Range(shakeMinIntensity, shakeMaxIntensity);
    }
}
