using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OneByOneDieComponent : MonoBehaviour
{
    public float EverySeconds;
    private float deltaTime;

    private int dotCount;
    void OnCollisionStay(Collision other)
    {
        deltaTime += Time.deltaTime;
        
        if (other.transform.CompareTag("Player") && deltaTime >= EverySeconds)
        {
            dotCount++;
            other.collider.SendMessage("ApplyDamage", dotCount);
            deltaTime = 0;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player") && deltaTime >= EverySeconds)
        {
            deltaTime = 0;
            dotCount = 0;
        }
    }
}
