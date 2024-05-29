using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIScore : MonoBehaviour
{
    GameEventManager gameEventManager;
    TMP_Text text;

    private void Awake()
    {
        gameEventManager = GameEventManager.Instance;
        gameEventManager.AddEvent(GameStatus.GAMERESET, OnGameReset);
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnGameReset()
    {
        
    }

    public void OnUpdateScore(int value)
    {
        text.text = value.ToString();
    }
}
