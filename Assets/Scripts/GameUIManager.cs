using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public List<Life> lifeUI = new();
    public GameObject gameStartPanelUI;
    public GameObject gameStopPanelUI;
    public GameObject gameClearPanelUI;
    public GameObject gameOverPanelUI;


    public void NextScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void GameStop()
    {
        gameStopPanelUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameReStart()
    {
        gameStopPanelUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameUICheck()
    {
        GamePanelUICheck();
        LifeUICheck();
    }

    private void GamePanelUICheck()
    {
        gameStartPanelUI.SetActive(CardGameManager.Instance._state == GameState.GameReset);
        gameClearPanelUI.SetActive(CardGameManager.Instance._state == GameState.GameClear);
        gameOverPanelUI.SetActive(CardGameManager.Instance._state == GameState.GameOver);
    }

    private void LifeUICheck()
    {
        for (int i = 1; i <= lifeUI.Count; i++)
        {
            lifeUI[i - 1].LoseLifeCheck(CardGameManager.Instance.currentLife < i);
        }
    }
}
