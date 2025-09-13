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
    [Header("받아올 매니저")]
    public GameUIManager gameUIManager;
    public CardFlipManager cardFlipManager;
    public CardShuffleManager cardShuffleManager;

    [Header("게임 상태")]
    public GameState _state;

    private List<Card> selectCards = new();
    private List<Card> CorrectCards = new();

    private WaitForSeconds waitForSeconds = new(1f);
    private Card selectCard;


    [HideInInspector] public float currentShowTime;
    [HideInInspector] public float currentLife;

    [Header("시작 설정")]
    [SerializeField] private float maxLife;
    [SerializeField] private int sameSelectCount;
    [SerializeField] private float showTime;

    private void Start()
    {
        _state = GameState.GameReset;
    }

    //CardClickManager에서 선택한 카드를 받아와 뒤집는 함수 실행
    public void SelectCardInput(GameObject cardObject)
    {
        selectCard = cardObject.GetComponent<Card>();
        if (selectCard == null) return;

        if (_state != GameState.GamePlaying) return;
        GameSystem();
    }

    //시작 버튼을 누를 시 실행되는 함수
    public void CardShuffle()
    {
        if (_state != GameState.GameReset) return;

        _state = GameState.GameStart;
        GameSystem();
    }

    //재시작 버튼을 누를 시 실행되는 함수
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

    #region 게임 진행 함수 ---------------------

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

    #region 시작 함수 ---------------------

    //게임 시작 되었을 시 함수 실행
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

    //CardShuffleManager에서 게임 종료 함수 실행(임시)
    private void GameStop()
    {
        StopAllCoroutines();
        cardFlipManager.CardAllFlip(CardFlipManager.Back);
    }

    #endregion ---------------------------

    #region 카드 체크 함수 ---------------------

    //선택을 완료한 카드를 체크하는 함수
    private void CardChecking()
    {
        List<Card> cards = selectCards.FindAll(card => card.CardId == selectCards[0].CardId);

        if (cards.Count >= sameSelectCount) CardCorrect(selectCards);
        else CardUncorrect(selectCards);
    }

    //선택을 완료한 카드가 맞을 경우 실행하는 함수
    private void CardCorrect(List<Card> cards) 
    { 
        foreach (var card in cards) 
            CorrectCards.Add(card); 
    }

    //선택을 완료한 카드가 맞지 않을 경우 실행하는 함수
    private void CardUncorrect(List<Card> cards) 
    { 
        foreach (var card in cards) 
            card.CardAnimation(CardFlipManager.Back);
        
        currentLife--;
    }

    //선택을 완료한 후 다음 선택으로 넘기는 설정 함수
    private void CheckEndSetting()
    {
        selectCards.Clear();
        _state = GameState.GamePlaying;
    }

    #endregion ---------------------------

    #region 게임 상태 체크 함수 ---------------------

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

    //시작하고 난 뒤 카드를 일정 시간 동안 보여주는 함수
    IEnumerator CardShow(float duration)
    {
        cardFlipManager.CardAllFlip(CardFlipManager.Front);
        yield return ForTime(duration);
        cardFlipManager.CardAllFlip(CardFlipManager.Back);

        _state = GameState.GamePlaying;
    }

    //일정 시간 동안 기달리는 함수
    //보여주는 시간을 시각적으로 표현하기 위해 직접 제작함
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

    //선택을 완료한 카드를 체크하고 다음 선택으로 설정하는 함수
    IEnumerator CardCorrectCheck()
    {
        yield return waitForSeconds;
        CardChecking();
        CheckEndSetting();
        GameStateCheck();
    }

    
}
