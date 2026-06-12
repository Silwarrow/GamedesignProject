using UnityEngine;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] AudioSource ButtonClickSound;
    public GameObject container;

    public void OpenOptions()
    {
        ButtonClickSound.Play();
        container.SetActive(true);
    }

    public void CloseOptions()
    {
        ButtonClickSound.Play();
        container.SetActive(false);
    }
}
