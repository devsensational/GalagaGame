using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEnemyUnitPlacementGrid
{
    public void OnUnitPlace();
    public void OnUnitRemoveAt(int idx);
    public void OnResetGrid();
}
