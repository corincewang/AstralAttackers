using UnityEngine;

public class BossPartScript : MonoBehaviour
{
    [Header("Boss Part Settings")]
    public int maxHealth;
    public int currentHealth;
    public string partName = "BossPart";
    
    public BossScript bossScript;
    
    void Start()
    {
        bossScript = GetComponentInParent<BossScript>();
        
        if (bossScript != null)
        {
            if (partName == "Body")
            {
                maxHealth = bossScript.bodyHealth;
            }
            else if (partName == "Sword")
            {
                maxHealth = bossScript.swordHealth;
            }
            else if (partName == "WeakSpot")
            {
                maxHealth = bossScript.weakSpotHealth;
            }
        }
        
        currentHealth = maxHealth;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            Destroy(collision.gameObject);
            TakeDamage();
        }
    }
    
    public void OnChildHit()
    {
        TakeDamage();
    }
    
    private void TakeDamage()
    {     
        if (bossScript == null) return;
        
        if (partName == "Sword")
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                DestroySword();
            }
        }
        else if (partName == "WeakSpot" && bossScript.swordDestroyed)
        {
            bossScript.OnWeakSpotHit();
        }
        else if (partName == "Body" && bossScript.swordDestroyed && bossScript.weakSpotPhaseComplete)
        {
            bossScript.OnBodyHit();
        }
    }
    
    private void DestroySword()
    {
        bossScript.swordDestroyed = true;
        bossScript.CheckBossState();
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
        }
        rb.isKinematic = false;
        
        Destroy(GetComponent<BossPartScript>());
        Destroy(gameObject, 3f);
    }
}