using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFinishScreen : MonoBehaviour
{
    [SerializeField] AudioSource ButtonClickSound;
    public Text starsText;
    public void Setup(int stars)
    {
        gameObject.SetActive(true);
        starsText.text = "Stars: " + stars.ToString();
        Time.timeScale = 0f;
    }

    public void TryAgainButton()
    {
        ButtonClickSound.Play();
        StartCoroutine(LoadSceneAfterDelay(SceneManager.GetActiveScene().buildIndex));
    }

    public void NextLevelButton()
    {
        ButtonClickSound.Play();
        if(SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneAfterDelay(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else
        {
            // epische Endszene oder so
        }
    }

    public void ExitButton()
    {
        ButtonClickSound.Play();
        StartCoroutine(LoadSceneAfterDelay(0));
    }

    private IEnumerator LoadSceneAfterDelay(int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}