using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Collections;
public enum GameState
{
    Menu, Preround, Playing, PostRound, GameOver
}
public class GameManager : MonoBehaviour
{
    private int score;
    private int livesRemaining;
    private int LIVES_AT_START = 5;
    private GameObject currentMotherShip;
    private GameObject currentPlayer;
    private GameObject currentMoon;
    private bool moonUsed = false;

    public static GameManager Gary;
    public GameState state = GameState.Menu;
    public GameObject motherShipPrefab;
    public GameObject playerPrefab;
    public GameObject moonPrefab;
    public Vector3 moonStartPosition;

    // UI Variables
    public TextMeshProUGUI messageOverlay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI livesDisplay;
    void Awake()
    {
        if (Gary)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Gary = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GoToMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Menu && (Input.GetKeyDown(KeyCode.S)))
        {
            StartANewGame();
        }
    }

    private void GoToMenu()
    {
        state = GameState.Menu;

        messageOverlay.enabled = true;
        messageOverlay.text = "Press \"S\" to Start";
    }

    private void StartANewGame()
    {
        score = 0;
        livesRemaining = LIVES_AT_START;
        moonUsed = false;
        UpdateScoreDisplay();
        UpdateLivesDisplay();

        ResetRound();
    }

    private void ResetRound()
    {
        if (currentMotherShip)
        {
            Destroy(currentMotherShip);
        }
        
        if (currentPlayer)
        {
            Destroy(currentPlayer);
        }
        
        if (currentMoon)
        {
            Destroy(currentMoon);
        }
        
        currentMotherShip = Instantiate(motherShipPrefab);
        currentPlayer = Instantiate(playerPrefab);
        
        if (!moonUsed)
        {
            currentMoon = Instantiate(moonPrefab, moonStartPosition, Quaternion.identity);
        }
        
        StartCoroutine(GetReady());
    }

    private IEnumerator GetReady()
    {
        state = GameState.Preround;

        messageOverlay.enabled = true;
        messageOverlay.text = "Get Ready!!!";

        SoundManager.Steve.StartTheMusic();

        yield return new WaitForSeconds(3.0f);

        messageOverlay.enabled = false;

        StartRound();
    }

    private void StartRound()
    {
        state = GameState.Playing;

        currentMotherShip.GetComponent<MotherShipScript>().StartTheAttack();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreDisplay)
        {
            scoreDisplay.text = "Score: " + score;
        }
    }

    private void UpdateLivesDisplay()
    {
        if (livesDisplay)
        {
            livesDisplay.text = "Lives: " + livesRemaining;
        }
    }
    
    public void MoonWasShot(GameObject moonObject)
    {
        if (moonUsed) return;
        
        moonUsed = true;
        livesRemaining++;
        UpdateLivesDisplay();
        SoundManager.Steve.MakeHappySound();
        currentMoon = null;
        Destroy(moonObject);
    }

    public void ScheduleEnemyCheck()
    {
        StartCoroutine(CheckAndHandleWinCondition());
    }
    
    private IEnumerator CheckAndHandleWinCondition()
    {
        yield return new WaitForSeconds(0.2f);
        
        if (state == GameState.Playing && currentMotherShip)
        {
            int childCount = currentMotherShip.transform.childCount;
            
            if (childCount == 0)
            {
                currentMotherShip.GetComponent<MotherShipScript>().StopTheAttack();
                StartCoroutine(GameOverWinState());
            }
        }
    }

    public void EnemiesReachedGround()
    {
        if (state == GameState.Playing)
        {
            currentMotherShip.GetComponent<MotherShipScript>().StopTheAttack();
            StartCoroutine(OopsState());
        }
    }

    public void PlayerWasDestroyed()
    {

        SoundManager.Steve.PlayerExplosionSequence();

        currentMotherShip.GetComponent<MotherShipScript>().StopTheAttack();

        StartCoroutine(OopsState());

    }

    private IEnumerator GameOverWinState()
    {
        state = GameState.GameOver;
        
        messageOverlay.enabled = true;
        messageOverlay.text = "You Win! \nFinal Score: " + score + "\nPress R to Restart";
        
        while (!Input.GetKeyDown(KeyCode.R))
        {
            yield return null;
        }
        
        StartANewGame();
    }
    
    private IEnumerator GameOverLoseState()
    {
        state = GameState.GameOver;
        
        messageOverlay.enabled = true;
        messageOverlay.text = "Game Over! \nFinal Score: " + score + "\nPress R to Restart";
        
        while (!Input.GetKeyDown(KeyCode.R))
        {
            yield return null;
        }
        
        StartANewGame();
    }

    private IEnumerator OopsState()
    {
        state = GameState.PostRound;
        livesRemaining--;
        UpdateLivesDisplay();

        messageOverlay.enabled = true;
        messageOverlay.text = "You Failed.";

        yield return new WaitForSeconds(2f);

        if (livesRemaining > 0)
        {
            ResetRound();
        }
        else
        {
            StartCoroutine(GameOverLoseState());
        }
    }
}
