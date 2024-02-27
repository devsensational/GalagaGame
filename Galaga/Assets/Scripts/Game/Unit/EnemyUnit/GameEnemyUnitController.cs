using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemyUnitController : MonoBehaviour
{
    //Inspector
    [SerializeField]
    List<TextAsset>             StartPatternList;
    [SerializeField]
    List<TextAsset>             PatternList;
    [SerializeField]
    float                       EnemyUnitAttackIntervalTime;
    [SerializeField]
    float                       EnemyUnitMoveIntervalTime;

    //private
    List<GameObject>            enemyUnitTypeList;
    List<GameObject>            enemyUnitList;

    GameEnemyUnitPlacementGrid  unitGridObject;
    GameManager                 gameManager;
    GameStatus                  status;
    GameEventManager            gameEventManager;

    WaitForSeconds              enemyUnitAttackInterval;
    WaitForSeconds              enemyUnitMoveInterval;

    (byte, byte)[,]             unitGrid;

    private void OnGameReset()
    {
        status = GameStatus.GAMERESET;

        foreach(GameObject ptr in enemyUnitList) 
        { 
            Destroy(ptr);
        }

        enemyUnitList.Clear();
        unitGridObject.OnResetGrid();
        unitGrid = unitGridObject.UnitPlacementGrid;
        CreateUnitFromGrid();
        StartCoroutine(EnemyUnitFirstAttackCommand(0));
    }

    private void OnGameStart()
    {
        status = GameStatus.GAMESTART;
        
    }

    private void OnGameInProgress()
    {
        status = GameStatus.GAMEINPROGRESS;
        Debug.Log("(EnemyUnitController) GameInProgress");
    }

    private void OnGamePause()
    {

    }

    private IEnumerator EnemyUnitFirstAttackCommand(int idx)
    {
        yield return enemyUnitAttackInterval;
        Debug.Log("(EnemyUnitFirstAttackCommand) idx: " + idx);
        List<Vector3> pointers = CreatePathPointers(idx, StartPatternList);

        foreach (GameObject ptr in enemyUnitList)
        {
            GameEnemyUnit unitPtr = ptr.GetComponent<GameEnemyUnit>();
            Debug.Log("(EnemyUnitFirstAttackCommand) UnitSequence: " + unitPtr.UnitSeqeunceIdx);
            if (unitPtr.UnitSeqeunceIdx == idx)
            {
                unitPtr.StartUnitMove(pointers);
            }
            yield return enemyUnitMoveInterval;
        }

        if(StartPatternList.Count <= idx)
        {
            StartCoroutine(EnemyUnitFirstAttackCommand(idx + 1));
        }
    }

    private void EnemyUnitAttackCommand()
    {

    }

    private List<Vector3> CreatePathPointers(int idx, List<TextAsset> textAssets)
    {
        List<Vector3> pointers = new List<Vector3>();
        BezierObject bezierObject = FileUtilityManager.Instance.JsonUtil.LoadBezierFile<BezierObject>(textAssets[idx]);


        pointers.Add(new Vector3(bezierObject.StartPosition[0], bezierObject.StartPosition[1], bezierObject.StartPosition[2]));
        for (int i = 0; i < bezierObject.PointList.Count; i++)
        {
            pointers.Add(new Vector3(bezierObject.PointList[i][0], bezierObject.PointList[i][1], bezierObject.PointList[i][2]));
        }

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
            cnt++;

            //생성된 EnemyUnit에 대한 상태를 해당 주석 아래에 서술해야 함
            unitStatusPtr.UnitIndex             = idx;
            unitStatusPtr.EnemyUnitType         = unit.Item1;
            unitStatusPtr.UnitSeqeunceIdx       = unit.Item2;
            unitStatusPtr.UnitPosition          = unitGridObject.UnitPosition(idx);
        }

        unitGridObject.UnitCount = cnt;
    }

    public void RemoveUnit(int idx, GameObject ptr)
    {
        unitGridObject.UnitRemoveAt(idx);
        enemyUnitList.Remove(ptr);
        Destroy(ptr);
    }

    private void Init()
    {
        unitGridObject          = GetComponent<GameEnemyUnitPlacementGrid>();
        gameEventManager        = GameEventManager.Instance;
        gameManager             = GameManager.Instance;
        enemyUnitTypeList       = gameManager.EnemyUnitList;
        enemyUnitList           = new List<GameObject>();
        status                  = GameStatus.NONE;
        enemyUnitAttackInterval = new WaitForSeconds(EnemyUnitAttackIntervalTime);
        enemyUnitMoveInterval   = new WaitForSeconds(EnemyUnitMoveIntervalTime);

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
