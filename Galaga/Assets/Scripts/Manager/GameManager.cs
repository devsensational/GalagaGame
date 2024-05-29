using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoSingleton<GameManager>
{
    //Inspector
    [SerializeField] private GameObject              PlayerUnit;
    [SerializeField] public  List<GameObject>         EnemyUnitList;
    [SerializeField] private GameObject              EnemyUnitPlacementGrid;
    [SerializeField] private float                   GameWaitTimeValue;
    [SerializeField] private int                     FirstPlayerLife;

    //public
    public int Score        { get; private set; }
    public int StageLevel   { get; set; }
    public int PlayerLife   { get; set; }

    //private
    private GameEventManager        gameEventManager;
    private GameKeyManager          gameKeyManager;
    private GameObjectPoolManager   gamePoolManager;

    private GameEnemyUnitController gameEnemyUnitController;

    private GameStatus              gameStatus;

    private WaitForSeconds          gameWaitTime;

    //UI
    private GameObject ScoreValueUI;
    private GameObject StageValueUI;
    private GameObject PlayerLifeValueUI;

    protected override void ChildAwake()
    {
        DontDestroyOnLoad(this.gameObject);
        gameWaitTime = new WaitForSeconds(GameWaitTimeValue);
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
        gameEventManager.AddEvent(GameStatus.STAGECLEAR,        OnStageClear);

    }

    public void StartGameFirst()
    {
        gameEventManager.OnTriggerGameEvent(GameStatus.GAMERESET);
    }

    private IEnumerator SwitchGameStart()
    {
        yield return gameWaitTime;
        gameEventManager.OnTriggerGameEvent(GameStatus.GAMESTART);
    }

    private IEnumerator SwitchGameInProgress()
    {
        yield return gameWaitTime;
        gameEventManager.OnTriggerGameEvent(GameStatus.GAMEINPROGRESS);
    }

    private IEnumerator SwitchGameNextLevel()
    {
        yield return gameWaitTime;
        gameEventManager.OnTriggerGameEvent(GameStatus.GAMERESET);
    }

    public void OnAddScore(int score)
    {
        Score += score;
        ScoreValueUI.GetComponent<GameUIScore>().OnUpdateScore(Score);
    }

    private void OnGameReset()
    {
        Debug.Log("Game restart from GameManager");

        gameStatus = GameStatus.GAMERESET;

        ScoreValueUI = GameObject.Find("ScoreValue");
        StageValueUI = GameObject.Find("StageValue");
        PlayerLifeValueUI = GameObject.Find("LifeValue");

        StartCoroutine(SwitchGameStart());
    }

    private void OnGameStart()
    {
        Debug.Log("Game start from GameManager");

        gameStatus = GameStatus.GAMESTART;
        GameObject ptr = Instantiate(PlayerUnit);
        StageValueUI.GetComponent<GameUIStageValue>().OnUpdateStageValue(++StageLevel);
        ptr.name = "SpaceShip";

        StartCoroutine(SwitchGameInProgress());
    }

    private void OnStageClear()
    {
        Debug.Log("Game Stage Clear");
        StageLevel += 1;
        StartCoroutine(SwitchGameNextLevel());
    }

    private void OnGameProgress()
    {

    }
}
