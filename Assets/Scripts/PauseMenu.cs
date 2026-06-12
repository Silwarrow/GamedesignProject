using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;



public class PauseMenu : MonoBehaviour
{
    [SerializeField] AudioSource ButtonClickSound;
    public GameObject container;
    private bool screenPause = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (container.activeSelf)
            {
                container.SetActive(false);
                Time.timeScale = 1f;
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
        ButtonClickSound.Play();
        container.SetActive(false);
        Time.timeScale = 1;
        screenPause = false;
    }

    public void MainMenu()
    {
        ButtonClickSound.Play();
        StartCoroutine(LoadSceneAfterDelay(0));
        screenPause = false;
    }

    public void restartLevel()
    {
        ButtonClickSound.Play();
        StartCoroutine(LoadSceneAfterDelay(SceneManager.GetActiveScene().buildIndex));
        screenPause = false;
    }

    public void Options()
    {
        ButtonClickSound.Play();
    }

    public void QuitGame()
    {
        ButtonClickSound.Play();
        StartCoroutine(QuitAfterDelay());
        screenPause = false;
    }

    public int GetScreenStatus()
    {
        return screenPause ? 1 : 0;
    }

    private IEnumerator LoadSceneAfterDelay(int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    private IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
