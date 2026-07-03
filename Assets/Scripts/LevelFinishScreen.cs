using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFinishScreen : MonoBehaviour
{
    public Text starsText;
    public void Setup(int stars)
    {
        gameObject.SetActive(true);
        starsText.text = "Stars: " + stars.ToString();
        Time.timeScale = 0f;
    }

public void TryAgainButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevelButton()
    {
        Time.timeScale = 1f;
        if(SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}