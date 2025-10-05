using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Larry;

    public string levelName;
    public string nextLevel;
    void Awake()
    {
        Larry = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Gary.LevelStarted();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        if (Larry == this)
        {
            Larry = null;
        }
    }
}
