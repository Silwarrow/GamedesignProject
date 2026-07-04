using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource buttonClickSound;
    public void PlayGame()
    {
        buttonClickSound.Play();
        SceneManager.LoadScene("Level 1");
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
}
