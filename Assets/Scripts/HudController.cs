using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public Slider meltMeter;
    
    public void SetSliderValues(int minValue, int maxValue, int startValue)
    {
        meltMeter.minValue = minValue;
        meltMeter.maxValue = maxValue;
        meltMeter.value = startValue;
    }
    
    public void UpdateSliderValue(int newValue)
    {
        
    }

    public void ChangeImage()
    {
        
    }
}
