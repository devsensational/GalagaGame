using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameEnemyUnitController : MonoBehaviour
{
    //Inspector
    [SerializeField] private List<TextAsset>     StartPatternList;
    [SerializeField] private List<TextAsset>     PatternList;
    [SerializeField] private int                 NumberOfAttackEnemyUnit;
    [SerializeField] private float               EnemyUnitMoveIntervalTime;

    //private
    private List<GameObject>            enemyUnitTypeList;
    private List<GameObject>            enemyUnitList;
    private List<List<GameObject>>      enemyUnitSequence;

    private GameEnemyUnitPlacementGrid  unitGridObject;
    private GameManager                 gameManager;
    private GameStatus                  status;
    private GameEventManager            gameEventManager;
    private GameObject                  gamePlayerUnit;

    private GamePathGenerator           gamePathGenerator;

    private WaitForSeconds              enemyUnitAttackInterval;
    private WaitForSeconds              enemyUnitMoveInterval;

    private (byte, byte)[,]             unitGrid;

    private int                         unitCount;
    private int                         unitMoveCount;
    private int                         unitSequenceIdx;


    private void OnGameReset()
    {
        status = GameStatus.GAMERESET;

        foreach(GameObject ptr in enemyUnitList) 
        { 
            Destroy(ptr);
        }

        enemyUnitList.Clear();
        enemyUnitSequence.Clear();
        unitGridObject.OnResetGrid();
        unitGrid = unitGridObject.UnitPlacementGrid;
        unitSequenceIdx = 0;
        CreateUnitFromGrid();
    }

    private void OnGameStart()
    {
        status = GameStatus.GAMESTART;
        
    }

    private void OnGameInProgress()
    {
        status = GameStatus.GAMEINPROGRESS;
        gamePlayerUnit = GameObject.Find("SpaceShip");
        Debug.Log("(EnemyUnitController) GameInProgress");
        EnemyUnitFirstAttackCommand();
    }

    private void OnGamePause()
    {

    }

    private void EnemyUnitFirstAttackCommand()
    {
        unitMoveCount = enemyUnitSequence[unitSequenceIdx].Count;
        for(int i = 0; i < enemyUnitSequence[unitSequenceIdx].Count; i++)
        {
            Debug.Log("(EnemyUnitFirstAttackCommand) idx: " + i);
            GameEnemyUnit unitPtr = enemyUnitSequence[unitSequenceIdx][i].GetComponent<GameEnemyUnit>();
            if (unitPtr != null)
            { 
                List<Vector3> pointers = gamePathGenerator.CalculateBezierPathPoints(30, CreatePathPointers(unitSequenceIdx, StartPatternList, unitPtr.UnitPosition));
                unitPtr.StartUnitMove(pointers, EnemyUnitMoveIntervalTime * i);
            }
        }
    }

    private void EnemyUnitAttackCommand()
    {
        int NumberOfAttackEnemyUnit = this.NumberOfAttackEnemyUnit <= enemyUnitList.Count ? this.NumberOfAttackEnemyUnit : enemyUnitList.Count;
        unitMoveCount = NumberOfAttackEnemyUnit;
        for(int i = 0; i < NumberOfAttackEnemyUnit; i++)
        {
            int randomIdx = UnityEngine.Random.Range(0, enemyUnitList.Count);
            GameEnemyUnit unitPtr = enemyUnitList[randomIdx].GetComponent<GameEnemyUnit>();
            if(unitPtr != null)
            {
                Vector3 tempUnitPos = gamePlayerUnit.transform.position;
                tempUnitPos.y = tempUnitPos.y - 2f;

                List<Vector3> pointers = CreatePathPointers(0, unitPtr.PattenFile, unitPtr.UnitPosition, tempUnitPos);
                CalibratePointersBasedOnPlayerPos(pointers);
                pointers = gamePathGenerator.CalculateBezierPathPoints(30, pointers);

                unitPtr.StartUnitMove(pointers, EnemyUnitMoveIntervalTime * i);
                unitPtr.UnitAttack();
            }
            enemyUnitList.RemoveAt(randomIdx);
        }
    }

    private void CalibratePointersBasedOnPlayerPos(List<Vector3> pointers)
    {
        for(int i = 1; i < pointers.Count - 1; i++)
        {
            Debug.Log("(Calibrate)before: " + pointers[i].x);
            pointers[i] = new Vector3(pointers[i].x + gamePlayerUnit.transform.position.x, pointers[i].y, pointers[i].z);
            Debug.Log("(Calibrate)after: " + pointers[i].x);
        }
    }
     
    public void AddEnemyUnitList(GameObject gObject)
    {
        enemyUnitList.Add(gObject);
    }

    public bool EnemyUnitArrived()
    {
        unitMoveCount--;
        if(unitMoveCount == 0) 
        {
            if(unitSequenceIdx < enemyUnitSequence.Count - 1)
            {
                unitSequenceIdx++;
                Debug.Log("(EnemyUnitController) unitSequenceIdx = " + unitSequenceIdx);
                EnemyUnitFirstAttackCommand();
            }
            else if(enemyUnitList.Count > 0)
            {
                EnemyUnitAttackCommand();
            }
            return true;
        }
        return false;
    }

    private List<Vector3> CreatePathPointers(int idx, List<TextAsset> textAssets, Vector3 endPosition)
    {
        List<Vector3> pointers = new List<Vector3>();
        BezierObject bezierObject = FileUtilityManager.Instance.JsonUtil.LoadBezierFile<BezierObject>(textAssets[idx]);


        pointers.Add(new Vector3(bezierObject.StartPosition[0], bezierObject.StartPosition[1], bezierObject.StartPosition[2]));
        for (int i = 0; i < bezierObject.PointList.Count; i++)
        {
            pointers.Add(new Vector3(bezierObject.PointList[i][0], bezierObject.PointList[i][1], bezierObject.PointList[i][2]));
        }
        pointers.Add(endPosition);

        return pointers;
    }

    private List<Vector3> CreatePathPointers(int idx, List<TextAsset> textAssets, Vector3 StartPosition, Vector3 endPosition)
    {
        List<Vector3> pointers = new List<Vector3>();
        BezierObject bezierObject = FileUtilityManager.Instance.JsonUtil.LoadBezierFile<BezierObject>(textAssets[idx]);


        pointers.Add(StartPosition);
        for (int i = 0; i < bezierObject.PointList.Count; i++)
        {
            pointers.Add(new Vector3(bezierObject.PointList[i][0], bezierObject.PointList[i][1], bezierObject.PointList[i][2]));
        }
        pointers.Add(endPosition);

        return pointers;
    }

    private void CreateUnitFromGrid()
    {
        if (unitGridObject == null) { Debug.Log("UnitGridObject is Empty"); return; }
        if (unitGrid == null)       { Debug.Log("UnitGrid Array is Empty"); return; }

        GameObject ptr;
        GameEnemyUnit unitStatusPtr;
        int cnt = 0;
        int idx = -1;

        foreach ((byte, byte) unit in unitGrid)
        {
            idx++;
            if (unit.Item1 == 0) { continue; }

            ptr = Instantiate(enemyUnitTypeList[unit.Item1 - 1]);
            unitStatusPtr = ptr.GetComponent<GameEnemyUnit>();
            
            enemyUnitList.Add(ptr);
            while(enemyUnitSequence.Count <= unit.Item2) { enemyUnitSequence.Add(new List<GameObject>()); }
            enemyUnitSequence[unit.Item2].Add(ptr);
            cnt++;

            //생성된 EnemyUnit에 대한 상태를 해당 주석 아래에 서술해야 함
            unitStatusPtr.UnitIndex             = idx;
            unitStatusPtr.EnemyUnitType         = unit.Item1;
            unitStatusPtr.UnitSeqeunceIdx       = unit.Item2;
            unitStatusPtr.UnitPosition          = unitGridObject.UnitPosition(idx);
            unitStatusPtr.name                  = "EnemyUnit " + idx;
        }
        
        this.unitCount              = cnt;
        unitGridObject.UnitCount    = cnt;
    }

    public void RemoveUnit(int idx, GameObject ptr)
    {
        unitGridObject.UnitRemoveAt(idx);
        enemyUnitList.Remove(ptr);
        Destroy(ptr);
        unitCount--;
        if(unitCount <= 0)
        {
            gameEventManager.OnTriggerGameEvent(GameStatus.STAGECLEAR);
        }
    }

    private void Init()
    {
        unitGridObject          = GetComponent<GameEnemyUnitPlacementGrid>();
        gameEventManager        = GameEventManager.Instance;
        gameManager             = GameManager.Instance;
        enemyUnitTypeList       = gameManager.EnemyUnitList;
        enemyUnitList           = new List<GameObject>();
        enemyUnitSequence       = new List<List<GameObject>>();
        status                  = GameStatus.NONE;
        enemyUnitMoveInterval   = new WaitForSeconds(EnemyUnitMoveIntervalTime);
        gamePathGenerator       = new GamePathGenerator();
        unitSequenceIdx         = 0;


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
