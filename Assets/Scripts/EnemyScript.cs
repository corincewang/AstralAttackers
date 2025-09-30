using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public int scoreValue = 10;
    public GameObject bombPrefab;

    [HeaderAttribute("Enemy Swap Frames")]
    public GameObject enemyFrame1;
    public GameObject enemyFrame2;
    public GameObject enemyFrameExplode;

    [HeaderAttribute("Explosion Parameters")]
    public float explosionForce;

    public float explosionRadius;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyFrame1.SetActive(true);
        enemyFrame2.SetActive(false);
        enemyFrameExplode.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            DropABomb();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SwapFrames();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            makeExplosion();
        }
    }

    public void SwapFrames()
    {
        // reverse the active values of frame1 and 2
        enemyFrame1.SetActive(!enemyFrame1.activeSelf);
        enemyFrame2.SetActive(!enemyFrame2.activeSelf);
    }

    public void DropABomb()
    {
        GameObject bomb = Instantiate(bombPrefab, (transform.position + Vector3.down), Quaternion.identity);
        StartCoroutine(TrackBombForGroundHit(bomb));
    }
    
    private IEnumerator TrackBombForGroundHit(GameObject bomb)
    {
        float groundLevel = 0.26f; 
        bool hasReachedGround = false;
        
        while (bomb != null)
        {
            float currentY = bomb.transform.position.y;
            
            if (!hasReachedGround && currentY <= groundLevel)
            {
                hasReachedGround = true;
                yield return new WaitForSeconds(2f);
                
                if (bomb != null) 
                {
                    Destroy(bomb);
                }
                yield break;
            }
            
            yield return null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            //destroy bullet immediately
            Destroy(collision.gameObject);

            GameManager.Gary.AddScore(scoreValue);
            Destroy(this.gameObject, 0.2f);

            makeExplosion();

            SoundManager.Steve.MakeEnemyExplosionSound();
        }
    }

    private void makeExplosion()
    {
        enemyFrame1.SetActive(false);
        enemyFrame2.SetActive(false);
        enemyFrameExplode.SetActive(true);

        Rigidbody[] enemyBlocks = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody block in enemyBlocks)
        {
            block.AddExplosionForce(explosionForce, (transform.position + Vector3.back * 3f + Random.onUnitSphere), explosionRadius, 0f, ForceMode.Impulse);
        }
        
        // make the explode frame an orphan
        enemyFrameExplode.transform.parent = null;


        GameManager.Gary.ScheduleEnemyCheck();
        
        Destroy(this.gameObject, 0.1f);
    }
}
