using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class SpriteChanger : MonoBehaviour
{
    public Slider meltBar;
    public Sprite[] BarSprites;
    [SerializeField] private Image backgroundImage;


    void Update() //switch Sprites
    {
        ChangeSprite();
    }

    void ChangeSprite()
    {
        if (meltBar.value <= -72.9f)
        {
            backgroundImage.sprite = BarSprites[0]; //smallRed
        }
        
        if (meltBar.value > -72.9f && meltBar.value < -28.8f)
        {
            backgroundImage.sprite = BarSprites[1]; //smallYellow
        }

        if (meltBar.value > -28.8f && meltBar.value < 17.5)
        {
            backgroundImage.sprite = BarSprites[2]; //mediumGreen
        }

        if (meltBar.value > 17.5 && meltBar.value < 72.9f)
        {
            backgroundImage.sprite = BarSprites[3]; //bigYellow
        }

        if (meltBar.value > 72.9f)
        {
            backgroundImage.sprite = BarSprites[4]; //bigRed
        }
    }
}
