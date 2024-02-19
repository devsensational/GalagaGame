using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemyUnitPlacementGrid : MonoBehaviour
{
    //Inspector
    public TextAsset UnitPlacementFile;

    //public
    public byte[,]   UnitPlacementGrid {  get; private set; }

    //private
    private List<GameObject> UnitList;

    void Start()
    {
        UnitPlacementGrid = FileUtilityManager.Instance.CSVUtil.ReadCSV(UnitPlacementFile);
        UnitList          = GameManager.Instance.EnemyUnitList;
    }

}
