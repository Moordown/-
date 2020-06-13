using UnityEngine;
 
public class ActivateFromTriggerAndSetParent : Triggerable
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
        switch (action)
        {
            case TriggerAction.Activate:
                activateSelf = true;
                gameObject.SetActive(true);
                break;
            case TriggerAction.Deactivate:
                activateSelf = false;
                gameObject.SetActive(false);
                break;
            default:
                activateSelf = !activateSelf;
                gameObject.SetActive(activateSelf);
                break;
        }
    }
}
