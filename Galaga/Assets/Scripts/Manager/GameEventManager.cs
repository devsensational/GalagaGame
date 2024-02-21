using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameStatus 
{
    NONE    = 0,
    UNKNOWN,

    //아래에 Enum 값 작성
    GAMESTART,
    GAMEINPROGRESS,
    GAMERESET,
    GAMEPAUSE,
    GAMEEXIT,
    PLAYERDEAD,
    PLAYERCAPTURED,
    STAGECLEAR,
    GAMEOVER,
    //Enum End

    END = 99
}


public class GameEventManager : MonoSingleton<GameEventManager>
{
    public Dictionary<GameStatus, funcType1> EventDictionary { get; private set; }

    public delegate void funcType1();

    public GameStatus GameStatusNow { get; private set; }
    public void AddEvent(GameStatus status, funcType1 func)
    {
        if (checkEventDictionaryIsNull()) return;

        // 키가 존재하며 대리자가 아직 추가되지 않았는지 확인
        if (EventDictionary.ContainsKey(status))
        {
            if (EventDictionary[status] != null && !EventDictionary[status].GetInvocationList().Contains(func))
            {
                EventDictionary[status] += func;
            }
        }
        else
        {
            EventDictionary.Add(status, null);
            EventDictionary[status] += func;
        }
    }

    public void RemoveEvent(GameStatus status, funcType1 func)
    {
        if (checkEventDictionaryIsNull()) return;
        if (!EventDictionary.ContainsKey(status)) return;
        EventDictionary[status] -= func;
    }

    public void OnTriggerGameEvent(GameStatus status)
    {
        if (checkEventDictionaryIsNull()) return;
        if (!EventDictionary.ContainsKey(status))
        {
            Debug.Log("This status empty");
            return;
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
