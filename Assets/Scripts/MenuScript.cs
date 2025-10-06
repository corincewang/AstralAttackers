using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    void Start()
    {

    }
    public void btn_StartTheGame()
    {
        SceneManager.LoadScene("Level01");
    }
    
    public void btn_GoToTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }
    
    public void btn_GoToCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
    
    public void btn_GoBackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    
    public void btn_QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
