using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : Triggerable
{
    public GameObject TimeOutNotifier;
    public int SecondsForTimeOut;
    public Text TimePanel;

    private IEnumerator ticTac;
    
    public override void Trigger(TriggerAction action)
    {
        if (action == TriggerAction.Activate)
        {
            if (ticTac != null)
                StopCoroutine(ticTac);
            ticTac = StartBackTicTac(SecondsForTimeOut);
            StartCoroutine(ticTac);
            TimePanel.gameObject.SetActive(true);
        } else if (action == TriggerAction.Deactivate)
        {
            if (ticTac != null)
                StopCoroutine(ticTac);
            ticTac = null;
            TimePanel.gameObject.SetActive(false);
        }
    }

    IEnumerator StartBackTicTac(int time)
    {
        while (time > 0)
        {
            TimePanel.text = GetTimerFormat(time);
            if (time <= 10)
                TimePanel.color = Color.red;
            time--;
            yield return new WaitForSeconds(1);
        }

        foreach (var trigger in TimeOutNotifier.GetComponents<Trigger>())
            trigger.TriggerTargets();
        
        TimePanel.gameObject.SetActive(false);
    }

    string GetTimerFormat(int time)
    {
        return $"{time/60/10}{time/60%10}:{time%60/10}{time%60%10}";
    }
}
