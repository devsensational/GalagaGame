using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemyUnitController : MonoBehaviour
{
    //private
    List<GameObject>            enemyUnitTypeList;
    List<GameObject>            enemyUnitList;

    GameEnemyUnitPlacementGrid  unitGridObject;
    GameManager                 gameManager;
    GameStatus                  status;
    GameEventManager            gameEventManager;

    byte[,]                     unitGrid;

    private void OnGameReset()
    {
        status = GameStatus.GAMERESET;
        unitGridObject.OnResetGrid();
        unitGrid = unitGridObject.UnitPlacementGrid;
        CreateUnitFromGrid();
    }

    private void OnGameStart()
    {

    }

    private void OnGamePause()
    {

    }

    private void CreateUnitFromGrid()
    {
        if (unitGridObject == null) { Debug.Log("UnitGridObject is Empty"); return; }
        if (unitGrid == null)       { Debug.Log("UnitGrid Array is Empty"); return; }

        GameObject ptr;
        int cnt = 0;

        foreach (byte unit in unitGrid) 
        {
            if(unit == 0) { continue; }
            ptr = Instantiate(enemyUnitTypeList[unit - 1]);
            enemyUnitList.Add(ptr);
            cnt++;

            //생성된 EnemyUnit에 대한 상태를 해당 주석 아래에 서술해야 함

        }

        unitGridObject.UnitCount = cnt;
    }

    private void Init()
    {
        unitGridObject      = GetComponent<GameEnemyUnitPlacementGrid>();
        gameEventManager    = GameEventManager.Instance;
        gameManager         = GameManager.Instance;
        enemyUnitTypeList   = gameManager.EnemyUnitList;
        enemyUnitList       = new List<GameObject>();
        status              = GameStatus.NONE;

        gameEventManager.AddEvent(GameStatus.GAMERESET, OnGameReset);
        gameEventManager.AddEvent(GameStatus.GAMESTART, OnGameStart);
        gameEventManager.AddEvent(GameStatus.GAMEPAUSE, OnGamePause);
    }

    private void Start()
    {
        Init();
    }
}
