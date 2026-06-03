using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    public Slider meltBar;
    public Sprite[] BarSprites;
    [SerializeField] private Image backgroundImage;
    private int stars = 0;


    void Update() //switch Sprites
    {
        ChangeSprite();
    }

    void ChangeSprite()
    {
        if (meltBar.value <= -72.9f)
        {
            backgroundImage.sprite = BarSprites[0]; //smallRed
            stars = 1;
        }
        
        if (meltBar.value > -72.9f && meltBar.value < -28.8f)
        {
            backgroundImage.sprite = BarSprites[1]; //smallYellow
            stars = 2;
        }

        if (meltBar.value > -28.8f && meltBar.value < 17.5)
        {
            backgroundImage.sprite = BarSprites[2]; //mediumGreen
            stars = 3;
        }

        if (meltBar.value > 17.5 && meltBar.value < 72.9f)
        {
            backgroundImage.sprite = BarSprites[3]; //bigYellow
            stars = 2;
        }

        if (meltBar.value > 72.9f)
        {
            backgroundImage.sprite = BarSprites[4]; //bigRed
            stars = 1;
        }
    }
    public int GetStars()
    {
        return stars;
    }
}
