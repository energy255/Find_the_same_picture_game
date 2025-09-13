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
        //�Լ� �⺻ ����
        int ShuffleCount = Cards.Count / sameSelectCount;
        shuffleCardDatas.Clear();

        //ī�� �����͸� ���� �� shuffleCardDatas�� �� ����
        ShuffleList(ShuffleCount, startCardDatas);
        SameCartCount(ShuffleCount, sameSelectCount);

        //������ ī�� ���� �� �ð��� ī�忡 �̹���, ���̵� �� ������ ����
        ShuffleList(shuffleCardDatas.Count, shuffleCardDatas);
        CardSetting();
    }

    //ī�� ������ ����
    private void CardSetting()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].CardSetting(shuffleCardDatas[i].CardImage, shuffleCardDatas[i].CardId);
        }
    }

    //���� ī�尡 �־���� ��ŭ ī�� ������ ����
    private void SameCartCount(int ShuffleCount, int sameCartCount)
    {
        for (int i = 0; i < sameCartCount; i++)
        {
            AddList(ShuffleCount, startCardDatas, shuffleCardDatas);
        }
    }

    //� ����Ʈ�� ���� Ƚ�� ��ŭ �ߺ����� �ʰ� ����
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

    //� ����Ʈ�� ���� ���� Ƚ�� ��ŭ �ٸ� ����Ʈ�� ���� ����
    private void AddList<T>(int ShuffleCount, List<T> SendList, List<T> TargetList)
    {
        for (int i = SendList.Count; i > SendList.Count - ShuffleCount; i--)
        {
            TargetList.Add(SendList[i - 1]);
        }
    }
}
