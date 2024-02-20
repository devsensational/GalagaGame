using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemyUnitPlacementGrid : MonoBehaviour, IGameEnemyUnitPlacementGrid
{
    //Inspector
    public TextAsset UnitPlacementFile;

    //public
    public byte[,]   UnitPlacementGrid {  get; private set; }

    //private 
    private List<GameObject>    unitList;
    private GameEventManager    gameEventManager;
    private int                 width;
    private int                 height;
    private int                 UnitCount;

    public void OnUnitPlace()
    {

    }

    public void OnUnitRemoveAt(int idx)
    {
        int row, col;
        CalculateUnitPosition(idx, out row, out col);
        UnitPlacementGrid[row, col] = 0;
    }

    public void OnResetGrid()
    {
        UnitPlacementGrid = FileUtilityManager.Instance.CSVUtil.ReadCSV(UnitPlacementFile);
    }

    private void CalculateUnitPosition(int idx, out int row, out int col)
    {
        row = idx / width; col = idx % height;
    }

    private void Init()
    {
        UnitPlacementGrid   = FileUtilityManager.Instance.CSVUtil.ReadCSV(UnitPlacementFile);
        unitList            = GameManager.Instance.EnemyUnitList;
        gameEventManager    = GameEventManager.Instance;
        width               = UnitPlacementGrid.GetLength(0);
        height              = UnitPlacementGrid.GetLength(1);

        gameEventManager.AddEvent(GameStatus.GAMERESET, OnResetGrid);
    }

    void Start()
    {

    }

}
