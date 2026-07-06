using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] AudioSource buttonClickSound;
    public GameObject container;
    public void LoadLevel1()
    {
        buttonClickSound.Play();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }

    public void LoadLevel2()
    {
        buttonClickSound.Play();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2);
    }

    public void LoadLevel3()
    {
        buttonClickSound.Play();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(3);
    }

    public void Quit()
    {
        buttonClickSound.Play();
        container.SetActive(false);
    }

    public void PlayButtonSound()
    {
        buttonClickSound.Play();
    }
}
