using UnityEngine;

public class PushInTie : MonoBehaviour
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
    }

    private void OnCollisionEnter(Collision other)
    {
        var tied = other.gameObject.GetComponent<PushInTied>();
        if (tied is null) return;
        target = tied.GetComponent<Transform>();
        push = tied.PushVector;
    }

    private void OnCollisionExit(Collision other)
    {
        var tied = other.gameObject.GetComponent<PushInTied>();
        if (tied is null) return;
        target = null;
    }
}