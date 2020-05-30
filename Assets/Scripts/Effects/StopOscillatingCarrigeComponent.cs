using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(CarriageOscillatingComponent))]
public class StopOscillatingCarrigeComponent : MonoBehaviour
{
    private CarriageOscillatingComponent oscillatingComponent;
    
    void Start()
    {
        oscillatingComponent = gameObject.GetComponent<CarriageOscillatingComponent>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        oscillatingComponent.IsStopMoving = true;
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        oscillatingComponent.IsStopMoving = false;
    }
}
