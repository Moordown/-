using UnityEngine;

public class ActivateFromTrigger : Triggerable
{
    public bool activateSelf = false;
    private void Awake()
    {
        gameObject.SetActive(activateSelf);
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
