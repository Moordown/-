using System.Collections;
using UnityEngine;

public class Train : Triggerable
{
    public float TrainSpeed;

    private IEnumerator trainMoveCorutine;

    public override void Trigger(TriggerAction action)
    {
        if (action == TriggerAction.Activate && trainMoveCorutine == null)
        {
            trainMoveCorutine = RunTrain();
            StartCoroutine(trainMoveCorutine);
        } else if (action == TriggerAction.Deactivate && trainMoveCorutine != null)
        {
            StopCoroutine(trainMoveCorutine);
            trainMoveCorutine = null;
        }
    }

    IEnumerator RunTrain()
    {
        var ret = new object();
        while (true)
        {
            transform.position += Vector3.right * (TrainSpeed * Time.deltaTime);
            yield return ret;
        }
    }
}