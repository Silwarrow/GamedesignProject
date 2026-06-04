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
    }

public void TryAgainButton()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevelButton()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            //epische Endszene oder so
        }
    }

    public void ExitButton()
    {
        SceneManager.LoadSceneAsync(0);
    }
}