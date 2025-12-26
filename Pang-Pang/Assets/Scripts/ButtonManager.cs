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

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MoveToMainmenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ButtonSoundOn()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
    }
}
