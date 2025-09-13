using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GameReset,
    GameStart,
    CardChecking,
    GamePlaying,
    GameOver,
    GameClear
}

public class CardGameManager : Singleton<CardGameManager>
{
    [Header("�޾ƿ� �Ŵ���")]
    public GameUIManager gameUIManager;
    public CardFlipManager cardFlipManager;
    public CardShuffleManager cardShuffleManager;

    [Header("���� ����")]
    public GameState _state;

    private List<Card> selectCards = new();
    private List<Card> CorrectCards = new();

    private WaitForSeconds waitForSeconds = new(1f);
    private Card selectCard;


    [HideInInspector] public float currentShowTime;
    [HideInInspector] public float currentLife;

    [Header("���� ����")]
    [SerializeField] private float maxLife;
    [SerializeField] private int sameSelectCount;
    [SerializeField] private float showTime;

    private void Start()
    {
        _state = GameState.GameReset;
    }

    //CardClickManager���� ������ ī�带 �޾ƿ� ������ �Լ� ����
    public void SelectCardInput(GameObject cardObject)
    {
        selectCard = cardObject.GetComponent<Card>();
        if (selectCard == null) return;

        if (_state != GameState.GamePlaying) return;
        GameSystem();
    }

    //���� ��ư�� ���� �� ����Ǵ� �Լ�
    public void CardShuffle()
    {
        if (_state != GameState.GameReset) return;

        _state = GameState.GameStart;
        GameSystem();
    }

    //����� ��ư�� ���� �� ����Ǵ� �Լ�
    public void GameReset()
    {
        _state = GameState.GameReset;
        GameSystem();
    }

    private void GameSystem()
    {
        GameResetCheck();
        GameStartSetting();
        SelectCard(selectCard);
        CardMatchCheck();
        GameStateCheck();
    }

    #region ���� ���� �Լ� ---------------------

    private void GameResetCheck()
    {
        if (_state != GameState.GameReset) return;

        GameStop();
        ResetSetting();
    }

    private void GameStartSetting()
    {
        if (_state != GameState.GameStart) return;

        GameStop();
        GameStart();
    }

    private void SelectCard(Card card)
    {
        if (_state != GameState.GamePlaying) return;

        if (selectCards.Contains(card) || CorrectCards.Contains(card)) return;

        cardFlipManager.SelectCardFlip(card);
        selectCards.Add(card);
        if (selectCards.Count >= sameSelectCount) _state = GameState.CardChecking;
    }

    private void CardMatchCheck()
    {
        if (_state != GameState.CardChecking) return;

        StartCoroutine(CardCorrectCheck());
    }

    #endregion ---------------------------

    #region ���� �Լ� ---------------------

    //���� ���� �Ǿ��� �� �Լ� ����
    private void GameStart()
    {
        ResetSetting();

        cardShuffleManager.ShuffleStart(sameSelectCount);
        StartCoroutine(CardShow(showTime));
    }

    private void ResetSetting()
    {
        selectCards.Clear();
        CorrectCards.Clear();
        currentLife = maxLife;
    }

    //CardShuffleManager���� ���� ���� �Լ� ����(�ӽ�)
    private void GameStop()
    {
        StopAllCoroutines();
        cardFlipManager.CardAllFlip(CardFlipManager.Back);
    }

    #endregion ---------------------------

    #region ī�� üũ �Լ� ---------------------

    //������ �Ϸ��� ī�带 üũ�ϴ� �Լ�
    private void CardChecking()
    {
        List<Card> cards = selectCards.FindAll(card => card.CardId == selectCards[0].CardId);

        if (cards.Count >= sameSelectCount) CardCorrect(selectCards);
        else CardUncorrect(selectCards);
    }

    //������ �Ϸ��� ī�尡 ���� ��� �����ϴ� �Լ�
    private void CardCorrect(List<Card> cards) 
    { 
        foreach (var card in cards) 
            CorrectCards.Add(card); 
    }

    //������ �Ϸ��� ī�尡 ���� ���� ��� �����ϴ� �Լ�
    private void CardUncorrect(List<Card> cards) 
    { 
        foreach (var card in cards) 
            card.CardAnimation(CardFlipManager.Back);
        
        currentLife--;
    }

    //������ �Ϸ��� �� ���� �������� �ѱ�� ���� �Լ�
    private void CheckEndSetting()
    {
        selectCards.Clear();
        _state = GameState.GamePlaying;
    }

    #endregion ---------------------------

    #region ���� ���� üũ �Լ� ---------------------

    private void GameStateCheck()
    {
        GameClearCheck();
        GameOverCheck();
        GameUIUpdate();
    }

    private void GameClearCheck()
    {
        if (cardShuffleManager.Cards.Count > CorrectCards.Count) return;

        _state = GameState.GameClear;
    }

    private void GameOverCheck()
    {
        if (currentLife > 0) return;

        _state = GameState.GameOver;
    }

    private void GameUIUpdate()
    {
        gameUIManager.GameUICheck();
    }

    #endregion ---------------------------

    //�����ϰ� �� �� ī�带 ���� �ð� ���� �����ִ� �Լ�
    IEnumerator CardShow(float duration)
    {
        cardFlipManager.CardAllFlip(CardFlipManager.Front);
        yield return ForTime(duration);
        cardFlipManager.CardAllFlip(CardFlipManager.Back);

        _state = GameState.GamePlaying;
    }

    //���� �ð� ���� ��޸��� �Լ�
    //�����ִ� �ð��� �ð������� ǥ���ϱ� ���� ���� ������
    IEnumerator ForTime(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentShowTime = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
    }

    //������ �Ϸ��� ī�带 üũ�ϰ� ���� �������� �����ϴ� �Լ�
    IEnumerator CardCorrectCheck()
    {
        yield return waitForSeconds;
        CardChecking();
        CheckEndSetting();
        GameStateCheck();
    }

    
}
