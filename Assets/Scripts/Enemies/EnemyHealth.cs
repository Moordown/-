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
    public void ReceiveCollision(ref Collision  col, ref string name)
    {
        if (col.transform.tag == "bullet")
        {
            Destroy(col.gameObject);
            Debug.Log(name);
            if (enemyMesh != null && currentHealth > 0)
            {
                for (int i = 0; i < weakSpotNames.Length; i++)
                {
                    if (name == weakSpotNames[i])
                    {
                        currentHealth -= weakSpotHealth[i];
                        StartCoroutine(HitFlash(i));
                    }
                }
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            currentHealth -= 1;
            Destroy(other.gameObject);
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