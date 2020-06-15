using UnityEngine;
public class WeakSpot : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public void OnCollisionEnter(Collision col)
    {
        string name = gameObject.name;
        enemyHealth.ReceiveCollision(ref col, ref name);
    }
}