using System;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
public class ElectricalSpore : Spore
{
    private LampPost[] LampPosts;

    void Awake()
    {
        LampPosts = FindObjectsOfType(typeof (LampPost)) as LampPost[];
    }

    public override void OnLeftClick()
    {
        foreach (var lampPost in LampPosts)
        {
            lampPost.LampLight.enabled = ! lampPost.LampLight.enabled;
        }
    }
}
