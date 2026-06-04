using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using UnityEngine.SceneManagement;

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

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                Level1Sprites();
                break;
            case 2:
                Level2Sprites();
                break;
            case 3:
                Level3Sprites();
                break;
        }
    }

    void Level1Sprites ()
    {
        if (meltBar.value <= -54.3)
        {
            backgroundImage.sprite = BarSprites[0]; //smallRed
        }
        
        if (meltBar.value > -54.3 && meltBar.value < 17.4)
        {
            backgroundImage.sprite = BarSprites[1]; //smallYellow
        }

        if (meltBar.value > 17.4 && meltBar.value < 82.8)
        {
            backgroundImage.sprite = BarSprites[2]; //bigGreen
        }

        if (meltBar.value > 82.8)
        {
            backgroundImage.sprite = BarSprites[3]; //bigRed
        }
    }

    void Level2Sprites ()
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

    void Level3Sprites ()
    {
        if (meltBar.value <= -87.4)
        {
            backgroundImage.sprite = BarSprites[0]; //smallRed
        }
        
        if (meltBar.value > -87.4 && meltBar.value < -33.9)
        {
            backgroundImage.sprite = BarSprites[1]; //smallGreen
        }

        if (meltBar.value > -33.9 && meltBar.value < 38.5)
        {
            backgroundImage.sprite = BarSprites[2]; //bigYellow
        }

        if (meltBar.value > 38.5)
        {
            backgroundImage.sprite = BarSprites[3]; //bigRed
        }
    }
}
