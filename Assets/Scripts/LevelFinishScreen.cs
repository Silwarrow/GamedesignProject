using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFinishScreen : MonoBehaviour
{
    [SerializeField] AudioSource buttonClickSound;
    [SerializeField] AudioSource levelCompleteSound;
    public Text starsText;
    public void Setup(int stars)
    {
        gameObject.SetActive(true);
        starsText.text = "Stars: " + stars.ToString();
        Time.timeScale = 0f;
        // Grappling Hook deaktivieren
        Grapple grapple = FindFirstObjectByType<Grapple>();
        if (grapple != null)
        {
            grapple.canGrapple = false;
        }
        levelCompleteSound.Play();
    }

public void TryAgainButton()
    {
        buttonClickSound.Play();
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevelButton()
    {
        buttonClickSound.Play();
        Time.timeScale = 1f;
        if(SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ExitButton()
    {
        buttonClickSound.Play();
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}