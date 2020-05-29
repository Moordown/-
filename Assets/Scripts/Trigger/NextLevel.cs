using UnityEngine;
 
public class NextLevel : Triggerable
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void Trigger(TriggerAction action)
    {
        if (action != TriggerAction.Activate) return;
        gameObject.SetActive(true);
    }
}
