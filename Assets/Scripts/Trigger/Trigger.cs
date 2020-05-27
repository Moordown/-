using System;
using System.Linq;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public TriggerAction TriggerAction = TriggerAction.Activate;
    public Triggerable[] targets;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TriggerTargets();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            TriggerTargets();
    }

    public void TriggerTargets()
    {
        foreach(var target in targets.Where(t => t != null))
            target.Trigger(TriggerAction);
    }

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.25f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (targets == null) return;
        foreach (var t in targets.Where(t => t != null))
            Gizmos.DrawLine(transform.position, t.transform.position);
    }
}