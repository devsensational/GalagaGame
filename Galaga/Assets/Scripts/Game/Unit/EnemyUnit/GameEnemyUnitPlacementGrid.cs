using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemyUnitPlacementGrid : MonoBehaviour, IGameEnemyUnitPlacementGrid
{
    //Inspector
    public TextAsset    UnitPlacementFile;
    public float        HorizontalSpacing;
    public float        VerticalSpacing;

    //public
    public byte[,] UnitPlacementGrid {  get; private set; }
    public int     UnitCount         { get; set; }

    //private
    private GameEventManager    gameEventManager;
    private int                 width;
    private int                 height;

    public void OnUnitPlace()
    {

    }

    public void UnitRemoveAt(int idx)
    {
        int row, col;
        CalculateUnitPosition(idx, out row, out col);
        UnitCount--;
        UnitPlacementGrid[row, col] = 0;
    }

    public void OnResetGrid()
    {
        UnitPlacementGrid = FileUtilityManager.Instance.CSVUtil.ReadCSV(UnitPlacementFile);
        Debug.Log("GameEnemyUnitPlaceGird reset complete");

    }

    private void CalculateUnitPosition(int idx, out int row, out int col)
    {
        row = idx / width; col = idx % height;
    }

    private void Init()
    {
        UnitPlacementGrid   = FileUtilityManager.Instance.CSVUtil.ReadCSV(UnitPlacementFile);
        gameEventManager    = GameEventManager.Instance;
        width               = UnitPlacementGrid.GetLength(0);
        height              = UnitPlacementGrid.GetLength(1);

        Debug.Log("GameEnemyUnitPlaceGird init complete");
    }

    void Start()
    {
        Init();
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

}
