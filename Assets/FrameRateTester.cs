using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateTester : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 120;
    }
}
