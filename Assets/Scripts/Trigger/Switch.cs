using UnityEngine;

[RequireComponent(typeof(Trigger))]
[RequireComponent(typeof(MeshRenderer))]
public class Switch : MonoBehaviour
{
// The materials we will swap between when the switch state changes
// Active means the switch CAN be pressed, inactive means it can't
    public Material activeMaterial;
    public Material inactiveMaterial;
    private Trigger _trigger;
    private MeshRenderer _renderer;
    private bool _pressed = false;

    void Awake()
    {
        _trigger = GetComponent<Trigger>();
        _renderer =
            GetComponent<MeshRenderer>();
        _renderer.sharedMaterial =
            activeMaterial;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_pressed && collision.gameObject.CompareTag("Player"))
        {
            _trigger.TriggerTargets();
            _renderer.sharedMaterial =
                inactiveMaterial;
            _pressed = true;
        }
    }
}