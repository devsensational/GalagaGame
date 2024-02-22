using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemyUnitController : MonoBehaviour
{
    //Inspector
    [SerializeField]
    List<TextAsset>             PatternList;
    float                       EnemyUnitAttackInterval;

    //private
    List<GameObject>            enemyUnitTypeList;
    List<GameObject>            enemyUnitList;

    GameEnemyUnitPlacementGrid  unitGridObject;
    GameManager                 gameManager;
    GameStatus                  status;
    GameEventManager            gameEventManager;

    WaitForSeconds              waitForSeconds;

    byte[,]                     unitGrid;

    private void OnGameReset()
    {
        status = GameStatus.GAMERESET;
        enemyUnitList.Clear();
        unitGridObject.OnResetGrid();
        unitGrid = unitGridObject.UnitPlacementGrid;
        CreateUnitFromGrid();
    }

    private void OnGameStart()
    {
        status = GameStatus.GAMESTART;
    }

    private void OnGameInProgress()
    {
        status = GameStatus.GAMEINPROGRESS;
        EnemyUnitAttackCommand();
    }

    private void OnGamePause()
    {

    }

    private void EnemyUnitFirstAttackCommand()
    {

    }

    private void EnemyUnitAttackCommand()
    {

        Invoke("EnemyUnitAttackCommand", EnemyUnitAttackInterval);
    }

    private void CreateUnitFromGrid()
    {
        if (unitGridObject == null) { Debug.Log("UnitGridObject is Empty"); return; }
        if (unitGrid == null)       { Debug.Log("UnitGrid Array is Empty"); return; }

        GameObject ptr;
        int cnt = 0;
        int idx = 0;

        foreach (byte unit in unitGrid)
        {
            idx++;
            if (unit == 0) { continue; }

            ptr = Instantiate(enemyUnitTypeList[unit - 1]);
            enemyUnitList.Add(ptr);
            cnt++;

            //생성된 EnemyUnit에 대한 상태를 해당 주석 아래에 서술해야 함
            ptr.GetComponent<GameEnemyUnit>().UnitIndex     = idx;
            ptr.GetComponent<GameEnemyUnit>().EnemyUnitType = unit;
        }

        unitGridObject.UnitCount = cnt;
    }

    public void RemoveUnit(int idx, GameObject ptr)
    {
        unitGridObject.UnitRemoveAt(idx);
        enemyUnitList.Remove(ptr);
    }

    private void Init()
    {
        unitGridObject      = GetComponent<GameEnemyUnitPlacementGrid>();
        gameEventManager    = GameEventManager.Instance;
        gameManager         = GameManager.Instance;
        enemyUnitTypeList   = gameManager.EnemyUnitList;
        enemyUnitList       = new List<GameObject>();
        PatternList         = new List<TextAsset>();
        status              = GameStatus.NONE;
        waitForSeconds      = new WaitForSeconds(EnemyUnitAttackInterval);

        gameEventManager.AddEvent(GameStatus.GAMERESET,         OnGameReset);
        gameEventManager.AddEvent(GameStatus.GAMESTART,         OnGameStart);
        gameEventManager.AddEvent(GameStatus.GAMEPAUSE,         OnGamePause);
        gameEventManager.AddEvent(GameStatus.GAMEINPROGRESS,    OnGameInProgress);

    }

    private void Start()
    {
        Init();
    }
}
