using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource ButtonClickSound;
    public void PlayGame()
    {
        ButtonClickSound.Play();
        StartCoroutine(LoadSceneAfterDelay("Level 1"));
    }

    public void QuitGame()
    {
        ButtonClickSound.Play();
        StartCoroutine(QuitAfterDelay());
    }

    public void Options()
    {
        ButtonClickSound.Play();
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadSceneAsync(sceneName);
    }

    private IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
