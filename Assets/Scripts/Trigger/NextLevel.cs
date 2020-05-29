using UnityEngine;
 
public class NextLevel : Triggerable
{
    public GameObject GameManager;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void Trigger(TriggerAction action)
    {
        if (action != TriggerAction.Activate) return;
        GameManager.GetComponent<GameManager>().enabled = false;

        gameObject.SetActive(true);
    }
}
