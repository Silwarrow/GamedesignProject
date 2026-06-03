using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
        // 0 = Main Menu
        // 1 = SampleScene
        // 2 = Level 1
        // 3 = Level 2
        // 4 = Level 3
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
