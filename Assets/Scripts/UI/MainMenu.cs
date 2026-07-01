using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("HomeScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
