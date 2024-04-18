using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUICurrentStatus : MonoBehaviour
{
    GameEventManager gameEventManager;
    TMP_Text text;

    private void Awake()
    {
        gameEventManager = GameEventManager.Instance;
        gameEventManager.AddEvent(GameStatus.GAMESTART, OnGameStart);
        gameEventManager.AddEvent(GameStatus.GAMERESET, OnGameReset);
        gameEventManager.AddEvent(GameStatus.GAMEINPROGRESS, OnGameInProgress);
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnGameStart()
    {
        text.text = "Game Start";
    }

    private void OnGameReset()
    {

    }

    private void OnGameInProgress()
    {
        text.text = "";

    }
}
