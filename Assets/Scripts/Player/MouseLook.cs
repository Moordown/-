using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private GameObject player;
    private float minClamp = -45;
    private float maxClamp = 45;
    [HideInInspector] public Vector2 rotation;
    private Vector2 currentLookRot;
    private Vector2 rotationV = new Vector2(0, 0);
    public float lookSensitivity = 2;
    public float lookSmoothDamp = 0.1f;

    void Start()
    {
        player = transform.parent.gameObject;
    }

    void Update()
    {
        Debug.LogWarning("1");
        rotation.y += Input.GetAxis("Mouse Y") * lookSensitivity;

        Debug.LogWarning("2");
        rotation.y = Mathf.Clamp(rotation.y, minClamp, maxClamp);
        
        Debug.LogWarning("3");
        player.transform.RotateAround(player.transform.position, Vector3.up, Input.GetAxis("Mouse X") * lookSensitivity);
        
        Debug.LogWarning("4");
        currentLookRot.y = Mathf.SmoothDamp(currentLookRot.y, rotation.y, ref rotationV.y, lookSmoothDamp);
        
        Debug.LogWarning("5");
        transform.localEulerAngles = new Vector3(-currentLookRot.y, 0, 0);
    }
}