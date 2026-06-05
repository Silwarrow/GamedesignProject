using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PauseMenu : MonoBehaviour
{
    public GameObject container;
    private bool screenPause = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (container.activeSelf)
            {
                Continue();
                screenPause = false;
            }
            
            else
            {
                container.SetActive(true);
                Time.timeScale = 0f;
                screenPause = true;
            }
        }
    }


    public void Continue()
    {
        container.SetActive(false);
        Time.timeScale = 1;
        screenPause = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        screenPause = false;
    }

    public void restartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        screenPause = false;
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public int GetScreenStatus()
    {
        return screenPause ? 1 : 0;
    }
}
