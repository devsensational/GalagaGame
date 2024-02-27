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
    public (byte, byte)[,]   UnitPlacementGrid {  get; private set; }
    public int                  UnitCount         { get; set; }

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
        UnitPlacementGrid[row, col].Item1 = 0;
    }

    public Vector3 UnitPosition(int idx)
    {
        int row, col;
        CalculateUnitPosition(idx, out row, out col);
        Vector3 newPosition = new Vector3(transform.position.x + (VerticalSpacing * col), transform.position.y - (HorizontalSpacing * row), transform.position.z);

        return newPosition;
    }

    public void OnResetGrid()
    {
        UnitPlacementGrid = FileUtilityManager.Instance.CSVUtil.ReadCSV(UnitPlacementFile);
        Debug.Log("GameEnemyUnitPlaceGird reset complete");

    }

    private void CalculateUnitPosition(int idx, out int row, out int col)
    {
        row = idx / width; col = idx % width;
    }

    private void Init()
    {
        UnitPlacementGrid   = FileUtilityManager.Instance.CSVUtil.ReadCSV(UnitPlacementFile);
        gameEventManager    = GameEventManager.Instance;
        width               = UnitPlacementGrid.GetLength(1);
        height              = UnitPlacementGrid.GetLength(0);

        Debug.Log("GameEnemyUnitPlaceGird init complete");
    }

    void Start()
    {
        Init();
        Debug.Log("Width: " + width + " / height: " + height);
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

}
