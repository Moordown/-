using UnityEngine;

public class TieToParent : Triggerable
{
    public Transform parentObject;
    public override void Trigger(TriggerAction action)
    {
        if (action == TriggerAction.Activate)
            transform.parent = parentObject;
    }
}
