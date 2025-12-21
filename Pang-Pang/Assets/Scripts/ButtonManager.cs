using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void GameStartButton()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
