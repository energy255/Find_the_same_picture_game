using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlipManager : MonoBehaviour
{
    public static readonly int Front = Animator.StringToHash("Front");
    public static readonly int Back = Animator.StringToHash("Back");

    //선택한 카드를 뒤집는 실행 함수
    public void SelectCardFlip(Card card)
    {
        card.CardAnimation(Front);
    }

    //모든 카드를 뒤집는 함수
    public void CardAllFlip(int trigerHash)
    {
        List<Card> card = CardGameManager.Instance.cardShuffleManager.Cards;
        for (int i = 0; i < card.Count; i++)
        {
            card[i].CardAnimation(trigerHash);
        }
    }
}
