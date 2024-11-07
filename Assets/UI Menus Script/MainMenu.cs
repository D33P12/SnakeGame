using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelTesting");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
