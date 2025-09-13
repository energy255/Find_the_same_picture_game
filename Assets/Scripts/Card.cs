using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Animator animator;
    public Sprite FrontImage;
    public Sprite BackImage;
    public int CardId;
    public bool isCorrectCard;
    private bool isShowCard;


    public void CardSetting(Sprite image, int cardId)
    {
        FrontImage = image;
        CardId = cardId;
    }

    public void CardAnimation(int triggerHash)
    {
        animator.SetTrigger(triggerHash);
    }

    public void CardFront()
    {
        isShowCard = true;
        CardImageChange();
    }

    public void CardBack()
    {
        isShowCard = false;
        CardImageChange();
    }

    private void CardImageChange()
    {
        GetComponent<Image>().sprite = isShowCard ? FrontImage : BackImage;
    }
}
