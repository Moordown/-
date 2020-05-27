using UnityEngine;
public class SendDamage : MonoBehaviour
{
    void OnCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            other.collider.SendMessage("ApplyDamage", 1);
        }
    }
}