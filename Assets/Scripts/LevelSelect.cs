using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] AudioSource ButtonClickSound;
    public GameObject container;
    public GameObject mainmenu;
    public void LoadLevel1()
    {
        ButtonClickSound.Play();
        StartCoroutine(LoadSceneAfterDelay(1));
    }

    public void LoadLevel2()
    {
        ButtonClickSound.Play();
        StartCoroutine(LoadSceneAfterDelay(2));
    }

    public void LoadLevel3()
    {
        ButtonClickSound.Play();
        StartCoroutine(LoadSceneAfterDelay(3));
    }

    public void Quit()
    {
        ButtonClickSound.Play();
        StartCoroutine(QuitDelay());
    }

    private IEnumerator LoadSceneAfterDelay(int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(0.4f);
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    private IEnumerator QuitDelay()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        container.SetActive(false);
        mainmenu.SetActive(true);
    }

    public void PlaySound()
    {
        ButtonClickSound.Play();
    }
}
