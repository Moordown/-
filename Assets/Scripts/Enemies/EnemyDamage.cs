using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int MaxHitNumber;
    private int currentHitNumber;

    private void OnEnable()
    {
        currentHitNumber = 0;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("bullet"))
            currentHitNumber++;
        if (currentHitNumber >= MaxHitNumber)
            gameObject.SetActive(false);
    }
}