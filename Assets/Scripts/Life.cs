using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public Image lifeImage;
    private Color loseLifeColor;

    public void LoseLifeCheck(bool isLoseLife)
    {
        loseLifeColor = lifeImage.color;
        loseLifeColor.a = isLoseLife ? 0.5f : 1f;
        lifeImage.color = loseLifeColor;
    }
}
