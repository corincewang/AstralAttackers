using UnityEngine;

public class BossScript : MonoBehaviour
{
    [Header("Boss Parts")]
    public GameObject body;
    public GameObject sword;
    public GameObject weakSpot;
    
    [Header("Boss Health")]
    public int bodyHealth = 5;
    public int swordHealth = 3;
    public int weakSpotHealth = 3;  
    
    [Header("Boss States")]
    public bool bodyDestroyed = false;
    public bool swordDestroyed = false;
    public bool weakSpotExposed = false;
    public bool weakSpotPhaseComplete = false;
    
    [Header("Visual Effects")]
    public Renderer[] bossRenderers;
    public Color flashColor = Color.white;
    public float flashDuration = 0.1f;
    
    [Header("Bomb Throwing")]
    public GameObject bombPrefab;
    public float bombInterval = 2f; 
    public float bombForce = 5f;
    private bool canThrowBombs = true;
    
    [Header("Weakspot Pulsation")]
    public float pulsateSpeed = 2f;
    private Renderer[] weakspotRenderers;
    
    void Start()
    {
        weakSpot.SetActive(false);
        
        StartCoroutine(BombThrowingRoutine());
    }
    

     public void CheckBossState()
    {
        if (swordDestroyed && !weakSpotExposed)
        {
            ExposeWeakSpot();
        }
    }
    
    private void ExposeWeakSpot()
    {
        weakSpotExposed = true;

        weakSpot.SetActive(true);
        SetupWeakspotPulsation();
        StartCoroutine(WeakspotPulsationRoutine());
    }
    
    public void OnWeakSpotHit()
    {
        if (!swordDestroyed) return;
        
        weakSpotHealth--;
        FlashBoss();
        
        if (weakSpotHealth <= 0 && !weakSpotPhaseComplete)
        {
            weakSpotPhaseComplete = true;
            DropWeakSpot();
        }
    }
    
    public void OnBodyHit()
    {
        if (!swordDestroyed || !weakSpotPhaseComplete) return;
        
        bodyHealth--;
        FlashBoss();
        
        if (bodyHealth <= 0)
        {
            DefeatBoss();
        }
    }
    
    private void FlashBoss()
    {
        StartCoroutine(FlashEffect());
    }
    
    private System.Collections.IEnumerator FlashEffect()
    {
        foreach (Renderer renderer in bossRenderers)
        {
            if (renderer != null)
            {
                renderer.material.color = flashColor;
            }
        }
        
        yield return new WaitForSeconds(flashDuration);
        
        foreach (Renderer renderer in bossRenderers)
        {
            if (renderer != null)
            {
                renderer.material.color = Color.white;
            }
        }
    }
    
    private void DropWeakSpot()
    {
        BossPartScript[] allParts = GetComponentsInChildren<BossPartScript>();
        
        foreach (BossPartScript part in allParts)
        {
            if (part.partName == "WeakSpot")
            {
                Rigidbody rb = part.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.isKinematic = false;
                
                Destroy(part.GetComponent<BossPartScript>());
                Destroy(part.gameObject, 3.0f);
            }
        }
    }

    
    private void DefeatBoss()
    {
        if (bodyDestroyed) return;
        
        bodyDestroyed = true;
        StopBombThrowing();
    
        MakePartsFall();
        Destroy(gameObject, 3.0f);
        StartCoroutine(TriggerWinAfterDelay());
    }
    
    private System.Collections.IEnumerator TriggerWinAfterDelay()
    {
        GameManager.Gary.AddScore(200);
        yield return new WaitForSeconds(1f);
        GameManager.Gary.SendMessage("TriggerBossWin");
    }
    
    
    private void MakePartsFall()
    {
        BossPartScript[] allParts = GetComponentsInChildren<BossPartScript>();
        
        foreach (BossPartScript part in allParts)
        {
            Rigidbody rb = part.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            
            Destroy(part.GetComponent<BossPartScript>());
        }
    }
    
    private System.Collections.IEnumerator BombThrowingRoutine()
    {
        while (canThrowBombs && !bodyDestroyed)
        {
            yield return new WaitForSeconds(bombInterval);
            
            if (canThrowBombs && !bodyDestroyed)
            {
                ThrowBomb();
            }
        }
    }
    
    private void ThrowBomb()
    {
        if (bombPrefab == null) return;
        
        GameObject bomb = Instantiate(bombPrefab, transform.position + Vector3.down, Quaternion.identity);
        
        Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
        if (bombRb != null)
        {
            bombRb.AddForce(Vector3.down * bombForce, ForceMode.Impulse);
        }
        
        Destroy(bomb, 3.0f);
    }
    
    public void StopBombThrowing()
    {
        canThrowBombs = false;
    }
    
    private void SetupWeakspotPulsation()
    {
        weakspotRenderers = weakSpot.GetComponentsInChildren<Renderer>();
    }
    
    private System.Collections.IEnumerator WeakspotPulsationRoutine()
    {
        float currentTime = 0f;
        float transitionDuration = 2f / pulsateSpeed;
        
        while (weakSpotExposed && !bodyDestroyed)
        {
            float lerpAmount = currentTime / transitionDuration;
            Color lerpedColor = Color.Lerp(Color.red, Color.yellow, lerpAmount);
            
            foreach (Renderer renderer in weakspotRenderers)
            {
                if (renderer != null)
                {
                    renderer.material.color = lerpedColor;
                }
            }
            
            currentTime += Time.deltaTime;
            
            if (currentTime >= transitionDuration)
            {
                currentTime = 0f;
            }
            
            yield return new WaitForEndOfFrame();
        }
    }

}
