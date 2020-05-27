using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Door : Triggerable
{
    public float moveSpeed = 5f;
    public Vector3 moveOffset;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _targetPosition;
    private Coroutine _update;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        // Transform the offset so that it works even when the door is rotated
        Vector3 offsetLocal = transform.TransformVector(moveOffset);
        _startPosition = transform.position;
        _endPosition = _startPosition + offsetLocal;
    }
    // Add the other functions here

    public override void Trigger(TriggerAction action)
    {
        // Support the door opening and closing
        if (action == TriggerAction.Toggle)
        {
            if (_targetPosition == _endPosition)
            {
                _targetPosition = _startPosition;
            }
            else
            {
                _targetPosition = _endPosition;
            }
        }
        else
        {
            if (action == TriggerAction.Deactivate)
            {
                _targetPosition = _startPosition;
            }
            else
            {
                _targetPosition = _endPosition;
            }
        }

        // Use a coroutine so we only update when the door is moving
        if (_update != null)
        {
            StopCoroutine(_update);
            _update = null;
        }

        _update = StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        while (true)
        {
// Calculate distance to the target position and also
// the distance we can move this frame
            Vector3 offset = _targetPosition - transform.position;
            float distance = offset.magnitude;
            float moveDistance = moveSpeed * Time.deltaTime;
// Keep moving towards target until we are close enough
            if (moveDistance < distance)
            {
                Vector3 move = offset.normalized * moveDistance;
                _rigidbody.MovePosition(transform.position + move);
                yield return null;
            }
            else
            {
                break;
            }
        }

// Ensure we move exactly to the target
        _rigidbody.MovePosition(_targetPosition);
        _update = null;
    }

    // Gizmos function for the Door
    void OnDrawGizmosSelected()
    {
        // Gizmos will be drawn using the local transform of the door
        // This means even if we rotate or scale the door, the preview
        // will be correct!
        Gizmos.matrix = transform.localToWorldMatrix;
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf != null)
        {
        // Setting Gizmos.matrix means we only need the offset here.Easy!
            Gizmos.DrawWireMesh(mf.sharedMesh, moveOffset);
        }
    }
}