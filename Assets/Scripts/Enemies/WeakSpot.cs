using UnityEngine;
public class WeakSpot : MonoBehaviour
{
    private GameObject recGO;
    private EnemyHealth _enemyHealth;
    private void Start()
    {
        recGO = transform.root.gameObject;
        _enemyHealth = recGO.
            GetComponent<EnemyHealth>();
    }
    public void OnCollisionEnter(Collision col)
    {
        string name = gameObject.name;
        _enemyHealth.ReceiveCollision(ref col, ref name);
    }
}