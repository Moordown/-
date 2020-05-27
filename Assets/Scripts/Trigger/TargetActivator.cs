using System;

public class TargetActivator : Triggerable
{
    public bool deactivateOnAwake = true;

    void Awake()
    {
        if (deactivateOnAwake)
            gameObject.SetActive(false);
    }

    public override void Trigger(TriggerAction action)
    {
        switch (action)
        {
            case TriggerAction.Activate:
                gameObject.SetActive(true);
                break;
            case TriggerAction.Deactivate:
                gameObject.SetActive(false);
                break;
            case TriggerAction.Toggle:
                gameObject.SetActive(!gameObject.activeSelf);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }        
    }
}