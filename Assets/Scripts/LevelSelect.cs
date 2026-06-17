using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public GameObject container;
    public void LoadLevel1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }

    public void LoadLevel2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2);
    }

    public void LoadLevel3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(3);
    }

    public void Quit()
    {
        container.SetActive(false);
    }
}
