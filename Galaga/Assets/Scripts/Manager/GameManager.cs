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
        gameEventManager    = GameEventManager.Instance;
        gameKeyManager      = GameKeyManager.Instance;
        gamePoolManager     = GameObjectPoolManager.Instance;

        gameEventManager.AddEvent(GameStatus.GameStart,         GameStart);
        gameEventManager.AddEvent(GameStatus.GameInProgress,    GameProgress);
    }

    private void GameStart(params object[] para)
    {
        Debug.Log("Game start from GameManager");
        Instantiate(PlayerUnit);
    }

    private void GameProgress(params object[] para)
    {

    }
}
