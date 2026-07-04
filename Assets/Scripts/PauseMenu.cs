using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PauseMenu : MonoBehaviour
{
    [SerializeField] AudioSource buttonClickSound;
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
        buttonClickSound.Play();
        container.SetActive(false);
        Time.timeScale = 1;
        screenPause = false;
    }

    public void MainMenu()
    {
        buttonClickSound.Play();
        SceneManager.LoadSceneAsync(0);
        screenPause = false;
    }

    public void restartLevel()
    {
        buttonClickSound.Play();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        screenPause = false;
    }

    public void QuitGame()
    {
        buttonClickSound.Play();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void PlayButtonSound()
    {
        buttonClickSound.Play();
    }

    public int GetScreenStatus()
    {
        return screenPause ? 1 : 0;
    }
}
