
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Trigger))]
public class FightJudger : MonoBehaviour
{
    private Trigger trigger;

    void Awake()
    {
        trigger = GetComponent<Trigger>();
    }

    public void OnDeath(GameObject obj)
    {
        trigger.TriggerTargets();
        StartCoroutine(InactivateObject(obj));
    }

    IEnumerator InactivateObject(GameObject obj)
    {
        yield return new WaitForSeconds(140f/30);
        obj.SetActive(false);
        obj.GetComponent<Collider>().enabled = false;
    }
}
