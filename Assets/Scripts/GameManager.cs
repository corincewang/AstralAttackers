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
    private MotherShipScript currentMotherShip;
    private GameObject currentPlayer;
    private GameObject currentMoon;
    private bool moonUsed = false;

    public static GameManager Gary;
    public GameState state = GameState.Menu;
    public GameObject playerPrefab;
    public GameObject moonPrefab;
    public Vector3 moonStartPosition;

    // UI Variables
    public TextMeshProUGUI messageOverlay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI livesDisplay;

    // level info
    public string currentLevel;

    void Awake()
    {
        if (Gary)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Gary = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartANewGame();
    }

    // Update is called once per frame
    void Update()
    {
        // if (state == GameState.Menu && (Input.GetKeyDown(KeyCode.S)))
        // {
        //     StartANewGame();
        // }
    }

    private void StartANewGame()
    {
        state = GameState.Menu;
        SoundManager.Steve.StopTheMusic();

        score = 0;
        livesRemaining = LIVES_AT_START;
        moonUsed = false;
        UpdateScoreDisplay();
        UpdateLivesDisplay();

        // ResetRound();
    }

    public void LevelStarted()
    {
        currentMotherShip = FindFirstObjectByType<MotherShipScript>();
        currentPlayer = FindFirstObjectByType<PlayerScript>().gameObject;
        StartCoroutine(GetReady());
    }

  

    private IEnumerator GetReady()
    {
        state = GameState.Preround;

        if (currentLevel != LevelManager.Larry.levelName)
        {
            messageOverlay.enabled = true;
            messageOverlay.text = LevelManager.Larry.levelName;

            yield return new WaitForSeconds(2f);

            currentLevel = LevelManager.Larry.levelName;
        }

        if (messageOverlay)
        {
            messageOverlay.enabled = true;
            messageOverlay.text = "Get Ready!!!";
        }

        if (SoundManager.Steve) SoundManager.Steve.StartTheMusic();

        yield return new WaitForSeconds(3.0f);

        if (messageOverlay) messageOverlay.enabled = false;

        StartRound();
    }

    private void StartRound()
    {
        state = GameState.Playing;

        currentMotherShip.StartTheAttack();
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
                currentMotherShip.StopTheAttack();
                StartCoroutine(GameOverWinState());
                
            }
        }
    }

    public void EnemiesReachedGround()
    {
        if (state == GameState.Playing)
        {
            currentMotherShip.StopTheAttack();
            StartCoroutine(OopsState());
        }
    }

    public void PlayerWasDestroyed()
    {

        SoundManager.Steve.PlayerExplosionSequence();

        currentMotherShip.StopTheAttack();

        StartCoroutine(OopsState());

    }

    private IEnumerator GameOverWinState()
    {
        state = GameState.GameOver;

        if (messageOverlay)
        {
            messageOverlay.enabled = true;
            messageOverlay.text = "You Win! \nFinal Score: " + score + "\nPress R to Restart";
        }

        yield return null;
    
        StartCoroutine(RoundWin());
    }

    private IEnumerator GameOverLoseState()
    {
        state = GameState.GameOver;

        if (messageOverlay)
        {
            messageOverlay.enabled = true;
            messageOverlay.text = "Game Over! \nFinal Score: " + score + "\nPress R to Restart";
        }

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

        if (messageOverlay)
        {
            messageOverlay.enabled = true;
            messageOverlay.text = "You Failed.";
        }

        yield return new WaitForSeconds(2f);

        if (livesRemaining > 0)
        {
            // ResetRound();
            LevelManager.Larry.ReloadLevel();
        }
        else
        {
            StartCoroutine(GameOverLoseState());
        }
    }

    private IEnumerator RoundWin()
    {
        state = GameState.PostRound;
        messageOverlay.enabled = true;
        messageOverlay.text = "Round Cleared!!";

        yield return new WaitForSeconds(2f);
        LevelManager.Larry.GoToNextLevel();
    }
}
