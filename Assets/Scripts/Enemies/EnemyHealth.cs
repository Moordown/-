using System.Collections;
using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    public int Health = 10;
    public int currentHealth;
    public GameObject enemyMesh;
    public Texture[] texture;
    public string[] weakSpotNames;
    public int[] weakSpotHealth;
    
    private int texRef;
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = Health;
    }
    
    public void Update()
    {
        if (currentHealth <= 0)
        {
            anim.SetBool(EnemyController.deadName, true);
            anim.SetBool(EnemyController.runName, false);
            anim.SetInteger(EnemyController.attackName, 0);
        }
    }
    public void ReceiveCollision(ref Collision  col, ref string weakPointName)
    {
        foreach (var contactPoint in col.contacts)
        {
            Debug.Log($"Try hit weak: {weakPointName} from {contactPoint.otherCollider.gameObject.name}");
            var damageable = contactPoint.otherCollider.gameObject.GetComponent<Damageable>();
            if (damageable is null) return;

            contactPoint.otherCollider.gameObject.SetActive(false);

            if (enemyMesh == null || currentHealth <= 0) return;
            for (var i = 0; i < weakSpotNames.Length; i++)
            {
                if (weakPointName != weakSpotNames[i]) continue;
                currentHealth -= weakSpotHealth[i];
                StartCoroutine(HitFlash(i));
            }
            Debug.Log($"hit weak: {weakPointName} from {contactPoint.otherCollider.gameObject.name}");
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        foreach (var contactPoint in other.contacts)
        {
            var otherGameObject = contactPoint.otherCollider.gameObject;
            var damageable = otherGameObject.GetComponent<Damageable>();
            if (damageable is null) continue;
            currentHealth -= damageable.damageValue;
            Debug.Log($"{currentHealth} after {contactPoint.otherCollider.transform.name} (-{damageable.damageValue})");
            otherGameObject.SetActive(false);
        }
    }
    private IEnumerator HitFlash(int num)
    {
        for (int i = 0; i < 5; i++)
        {
            enemyMesh.GetComponent<Renderer>().material.SetTexture("_EmissionMap", texture[num]);
            enemyMesh.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
            yield return new WaitForSeconds(0.1f);
            enemyMesh.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}