using System;
using UnityEngine;
using System.Collections;

public class LampPost : MonoBehaviour
{
    public Light LampLight { get; private set; }

    void Awake()
    {
        LampLight = gameObject.GetComponentInChildren<Light>();
    }
}
