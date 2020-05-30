using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OneByOneDieComponent : MonoBehaviour
{
    public float EverySeconds;
    private float deltaTime;
    void OnCollisionEnter(Collision other)
    {
        deltaTime += Time.deltaTime;
        
        if (other.transform.CompareTag("Player") && deltaTime >= EverySeconds)
        {
            other.collider.SendMessage("ApplyDamage", 1);
            deltaTime = 0;
        }
    }
}
