using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSquare : MonoBehaviour
{
    public GameObject Target { get; set; }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime);
    }
}
