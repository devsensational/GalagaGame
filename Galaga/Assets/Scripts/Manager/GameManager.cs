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
    public int Score        { get; private set; }
    public int StageLevel   { get; set; }

    //private
    private GameEventManager        gameEventManager;
    private GameKeyManager          gameKeyManager;
    private GameObjectPoolManager   gamePoolManager;
    private GameStatus              gameStatus;

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
        Score = 0;
        StageLevel = 0;

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

    public void OnAddScore(int score)
    {
        Score += score;
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
        GameObject ptr = Instantiate(PlayerUnit);
        ptr.name = "SpaceShip";

        gameEventManager.OnTriggerGameEvent(GameStatus.GAMEINPROGRESS);
    }

    private void OnGameProgress()
    {

    }
}
