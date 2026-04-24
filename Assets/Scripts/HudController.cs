using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public Text valueText;
    public int progress = 0;
    public Slider slider;

    public void OnSliderChange(float value)
    {
        valueText.text = value.ToString();
    }
    
    public void UpdateProgress()
    {
        progress++;
        slider.value = progress;
    }
}
