using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushOutTie : MonoBehaviour
{
    private Vector3 push;

    private bool tieIsActive;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!tieIsActive) return;
        _rigidbody.AddForce(push, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        var tied = other.gameObject.GetComponent<PushOutTied>();
        if (tied is null) return;
        tieIsActive = false;
        Debug.Log("enter out tie");
    }

    private void OnCollisionExit(Collision other)
    {
        var tied = other.gameObject.GetComponent<PushOutTied>();
        if (tied is null) return;
        push = tied.PushVector;
        tieIsActive = true;
        Debug.Log("exit out tie");
    }
}
