using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LooseScreen : MonoBehaviour
{
   public void RetryGame()
    {
        SceneManager.LoadScene("Level Testing");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
