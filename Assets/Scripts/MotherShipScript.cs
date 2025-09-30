using UnityEngine;
using System.Collections;

public class MotherShipScript : MonoBehaviour
{
    public int stepsToSide;
    public float sideStepUnits, downStepUnits;
    public float timeBetweenSteps, timeBetweenBombs;
    public float minTimeBetweenSteps = 0.2f;
    private int initialEnemyCount;
    private float GROUND_LEVEL = 2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialEnemyCount = transform.childCount; 
    }

    void Update()
    {

    }

    private float GetCurrentMoveSpeed()
    {
        int currentEnemyCount = transform.childCount;
        int enemiesDestroyed = initialEnemyCount - currentEnemyCount;
        
        // For every enemy destroyed, reduce wait time by a fixed amount
        float speedBoost = enemiesDestroyed * 0.05f; 
        
        float newSpeed = timeBetweenSteps - speedBoost;
        if (newSpeed < minTimeBetweenSteps) {
            newSpeed = minTimeBetweenSteps;
        }
            
        return newSpeed;
    }

    public void StartTheAttack()
    {
        StartCoroutine(MoveMother());
        StartCoroutine(DropOneBomb());
    }

    public void StopTheAttack()
    {
        StopAllCoroutines();
    }

    
    public IEnumerator MoveMother()
    {
        Vector3 sideStepVector = Vector3.right * sideStepUnits;
        Vector3 downStepVector = Vector3.down * downStepUnits;

        while (transform.childCount > 0)
        {
            // move side
            for (int i = 0; i < stepsToSide; i++)
            {
                transform.position += sideStepVector;

                BroadcastMessage("SwapFrames");

                SoundManager.Steve.MakeEnemyAdvanceSound();

                yield return new WaitForSeconds(GetCurrentMoveSpeed());
            }


            // move down
            transform.position += downStepVector;

            if (transform.position.y <= GROUND_LEVEL)
            {
                GameManager.Gary.EnemiesReachedGround();
                yield break; 
            }

            BroadcastMessage("SwapFrames");

            SoundManager.Steve.MakeEnemyAdvanceSound();

            yield return new WaitForSeconds(GetCurrentMoveSpeed());

            sideStepVector.x *= -1f;
        }
    }

    private IEnumerator DropOneBomb()
    {
        bool _isRunning = true;

        while (_isRunning)
        {
            yield return new WaitForSeconds(timeBetweenBombs);

            int enemyCount = transform.childCount;

            if (enemyCount > 0)
            {
                int enemyIndex = Random.Range(0, enemyCount);
                EnemyScript thisEnemyScript = transform.GetChild(enemyIndex).GetComponent<EnemyScript>();

                if (thisEnemyScript)
                {
                    thisEnemyScript.DropABomb();
                }
            }
            else
            {
                _isRunning = false;
            }
        }

        
    }
}
