using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShuffleManager : MonoBehaviour
{
    public List<Card> Cards = new();
    public List<CardData> startCardDatas = new();
    private List<CardData> shuffleCardDatas = new();

    public void ShuffleStart(int sameSelectCount)
    {
        //함수 기본 설정
        int ShuffleCount = Cards.Count / sameSelectCount;
        shuffleCardDatas.Clear();

        //카드 데이터를 섞은 뒤 shuffleCardDatas에 값 삽입
        ShuffleList(ShuffleCount, startCardDatas);
        SameCartCount(ShuffleCount, sameSelectCount);

        //지정된 카드 섞은 뒤 시각적 카드에 이미지, 아이디 등 데이터 삽입
        ShuffleList(shuffleCardDatas.Count, shuffleCardDatas);
        CardSetting();
    }

    //카드 데이터 설정
    private void CardSetting()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].CardSetting(shuffleCardDatas[i].CardImage, shuffleCardDatas[i].CardId);
        }
    }

    //같은 카드가 있어야할 만큼 카드 데이터 삽입
    private void SameCartCount(int ShuffleCount, int sameCartCount)
    {
        for (int i = 0; i < sameCartCount; i++)
        {
            AddList(ShuffleCount, startCardDatas, shuffleCardDatas);
        }
    }

    //어떤 리스트를 섞을 횟수 만큼 중복되지 않게 섞음
    private void ShuffleList<T>(int ShuffleCount, List<T> ShuffleList)
    {
        int n = ShuffleList.Count;
        while (n > ShuffleList.Count - ShuffleCount)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = ShuffleList[k];
            ShuffleList[k] = ShuffleList[n];
            ShuffleList[n] = value;
        }
    }

    //어떤 리스트의 값을 보낼 횟수 만큼 다른 리스트에 값을 저장
    private void AddList<T>(int ShuffleCount, List<T> SendList, List<T> TargetList)
    {
        for (int i = SendList.Count; i > SendList.Count - ShuffleCount; i--)
        {
            TargetList.Add(SendList[i - 1]);
        }
    }
}
