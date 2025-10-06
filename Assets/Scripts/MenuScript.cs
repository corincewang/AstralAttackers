using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Gary){
            Destroy(GameManager.Gary.gameObject);
        }
    }
    public void btn_StartTheGame()
    {
        SceneManager.LoadScene("Level01");
    }
    
    public void btn_GoToTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }
    
    public void btn_GoBackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
