using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus 
{
    NONE    = 0,
    UNKNOWN,

    //아래에 Enum 값 작성
    GameStart,
    GameInProgress,
    GameReset,
    GameStop,
    PlayerDead,
    PlayerCaptured,
    StageClear,
    GameOver,
    //Enum End

    End = 99
}


public class GameEventManager : MonoSingleton<GameEventManager>
{
    public Dictionary<GameStatus, funcType1> EventDictionary { get; private set; }

    public delegate void funcType1(params object[] para);

    public GameStatus GameStatusNow { get; private set; }
    public void AddEvent(GameStatus status, funcType1 func)
    {
        checkEventDictionaryIsNull();

        if(!EventDictionary.ContainsKey(status)) 
        {
            EventDictionary.Add(status, func);
        }

        EventDictionary[status] += func;
    }

    public void OnTriggerGameEvent(GameStatus status)
    {
        checkEventDictionaryIsNull();
        if(!EventDictionary.ContainsKey(status))
        {
            Debug.Log("This status empty");
        }

        EventDictionary[status]();
    }

    private bool checkEventDictionaryIsNull()
    {
        if (EventDictionary == null)
        {
            Debug.Log("EventDictionary is null");
            return true;
        }
        return false;
    }

    public void Init()
    {
        EventDictionary = new Dictionary<GameStatus, funcType1>();
        GameStatusNow = GameStatus.NONE;
    }

    protected override void ChildAwake()
    {
        DontDestroyOnLoad(this);
        Init();
    }

    protected override void ChildOnDestroy()
    {

    }
}
