using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoSingleton<GameManager>
{
    //Inspector
    [SerializeField] 
    private GameObject              PlayerUnit;
    [SerializeField]
    public List<GameObject>         EnemyUnitList;
    [SerializeField]
    private GameObject              EnemyUnitPlacementGrid;

    //public

    //private
    private GameEventManager        gameEventManager;
    private GameKeyManager          gameKeyManager;
    private GameObjectPoolManager   gamePoolManager;
    private GameStatus              gameStatus;
    private int Score               { get; set; }
    private int StageLevel          { get; set; }

    protected override void ChildAwake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    protected override void ChildOnDestroy()
    {

    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gameEventManager        = GameEventManager.     Instance;
        gameKeyManager          = GameKeyManager.       Instance;
        gamePoolManager         = GameObjectPoolManager.Instance;

        gameEventManager.AddEvent(GameStatus.GAMESTART,         OnGameStart);
        gameEventManager.AddEvent(GameStatus.GAMERESET,         OnGameReset);
        gameEventManager.AddEvent(GameStatus.GAMEINPROGRESS,    OnGameProgress);
    }

    public void StartGameFirst()
    {
        gameEventManager.OnTriggerGameEvent(GameStatus.GAMERESET);
    }

    private void OnGameReset()
    {
        Debug.Log("Game restart from GameManager");

        gameStatus = GameStatus.GAMERESET;
        gameEventManager.OnTriggerGameEvent(GameStatus.GAMESTART);
    }

    private void OnGameStart()
    {
        Debug.Log("Game start from GameManager");

        gameStatus = GameStatus.GAMESTART;
        Instantiate(PlayerUnit);
        //gameEventManager.OnTriggerGameEvent(GameStatus.GAMESTART);
    }

    private void OnGameProgress()
    {

    }
}
