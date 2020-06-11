using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAwayComponent : Triggerable
{
    public Camera Camera;
    public float MoveSpeed;
    
    IEnumerator MoveToTarget()
    {
        while (true)
        {
            Vector3 offset = Camera.transform.position - transform.position;
            float distance = offset.magnitude;
            float moveDistance = (MoveSpeed / distance) * Time.deltaTime;
            if (moveDistance < distance)
            {
                Vector3 move = offset.normalized * moveDistance;
                transform.position += move;
                yield return null;
            }
            else
            {
                break;
            }
        }

        transform.position = Camera.transform.position;
    }
    
    public override void Trigger(TriggerAction action)
    {
        if (action != TriggerAction.Activate) return;

        Debug.Log("YES");
        gameObject.GetComponent<MouseLook>().enabled = false;
        transform.parent = null;
        transform.rotation = Camera.transform.rotation;
        StartCoroutine(MoveToTarget());
    }
}
