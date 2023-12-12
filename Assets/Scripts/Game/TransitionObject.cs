using System;
using UnityEngine;

internal class TransitionObject : MonoBehaviour
{
    private readonly Vector3 _scaler = new(100f, 100f, 100f);

    private void Start()
    {
        transform.position = Vector3.one;
        transform.localScale = new Vector3(100f, 100f, 100f);
    }

    private void Update()
    {
        transform.localScale -= _scaler * Time.deltaTime;
        if (transform.localScale.x < 0)
        {
            Destroy(gameObject);
        }
    }
}