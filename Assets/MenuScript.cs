using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void btn_StartTheGame()
    {
        SceneManager.LoadScene("Level01");
    }
}
