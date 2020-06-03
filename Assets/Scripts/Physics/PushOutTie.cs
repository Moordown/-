using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushOutTie : MonoBehaviour
{
    private Transform target;
    private Vector3 push;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (target is null) return;
        rigidbody.AddForce(push, ForceMode.Impulse);
        Debug.Log("use");
    }

    private void OnCollisionEnter(Collision other)
    {
        var tied = other.gameObject.GetComponent<PushOutTied>();
        if (tied is null) return;
        Debug.Log("Enter pushouttied");
        target = null;
    }

    private void OnCollisionExit(Collision other)
    {
        var tied = other.gameObject.GetComponent<PushOutTied>();
        if (tied is null) return;
        target = tied.GetComponent<Transform>();
        push = tied.PushVector;
        Debug.Log($"Exit pushouttied with: {push}");
    }
}
