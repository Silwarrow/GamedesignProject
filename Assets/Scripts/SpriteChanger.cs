using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class HudController : MonoBehaviour
{
    public Slider meltBar;
    [SerializeField] Sprite[] BarSprite;
    [SerializeField] Sprite newSprite; //current Sprite
    

    void Update() //switch Sprites
    {
        if (meltBar.value <= -72.9)
        {
            newSprite = BarSprite[0];
            gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        
        if (meltBar.value > -72.9 && meltBar.value < -28.8)
        {
            newSprite = BarSprite[1];
            gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }

        if (meltBar.value > -28.8 && meltBar.value < 28.8)
        {
            newSprite = BarSprite[2];
            gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }

        if (meltBar.value > 28.8 && meltBar.value < 72.9)
        {
            newSprite = BarSprite[3];
            gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }

        if (meltBar.value >= 72.9)
        {
            newSprite = BarSprite[4];
            gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }
}
