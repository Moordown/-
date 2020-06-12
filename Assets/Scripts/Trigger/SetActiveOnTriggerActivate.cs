using UnityEngine;
 
public class SetActiveOnTriggerActivate : Triggerable
{
    public Transform parentObject;
    private bool activateSelf = false;
    private void Awake()
    {
        gameObject.SetActive(activateSelf);
        transform.SetParent(parentObject, true);
    }

    public override void Trigger(TriggerAction action)
    {
        if (action != TriggerAction.Activate) return;
        activateSelf = true;
        gameObject.SetActive(true);
    }
}
