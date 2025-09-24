using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject bombPrefab;
    public AudioClip bombDropSound, enemyExplosionSound;
    private AudioSource audioComponent;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioComponent = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            DropABomb();
        }
    }

    public void DropABomb()
    {
        Instantiate(bombPrefab, (transform.position + Vector3.down), Quaternion.identity);

        if (audioComponent != null && bombDropSound != null)
        {
            audioComponent.PlayOneShot(bombDropSound);
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            //make explosion sound
            if (audioComponent != null && enemyExplosionSound != null)
            {
                audioComponent.PlayOneShot(enemyExplosionSound);
            }

            //destroy bullet immediately
            Destroy(collision.gameObject);
            
            Destroy(this.gameObject, 0.3f);
        }
    }
}
