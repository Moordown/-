using System;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class IgnoreCollisionComponent : MonoBehaviour
{
    
    public Collider[] ignoreCollisionObjects;

    private void Awake()
    {
        var col = gameObject.GetComponent<Collider>();
        if (ignoreCollisionObjects is null) return;
        foreach (var ignoreCollisionObject in ignoreCollisionObjects)
            Physics.IgnoreCollision(col, ignoreCollisionObject);
    }
}
