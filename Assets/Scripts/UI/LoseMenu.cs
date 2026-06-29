using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenuScene");
    }
}
