using System.Collections;
using UnityEngine;

public class MoveOnTrigger : Triggerable
{
    public float TrainSpeed;
    public Vector3 Direction = Vector3.right;

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
            transform.position += Direction * (TrainSpeed * Time.deltaTime);
            yield return ret;
        }
    }
}