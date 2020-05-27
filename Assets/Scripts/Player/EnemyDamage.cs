using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private int hitNumber;

    private void OnEnable()
    {
        hitNumber = 0;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("bullet"))
        {
            hitNumber++;
        }
        if (hitNumber == 3)
        {
            gameObject.SetActive(false);
        }
    }
}