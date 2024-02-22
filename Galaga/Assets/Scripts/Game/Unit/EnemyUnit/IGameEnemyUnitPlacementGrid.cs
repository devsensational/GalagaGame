using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEnemyUnitPlacementGrid
{
    public void OnUnitPlace();
    public void OnResetGrid();
    public void UnitRemoveAt(int idx);
}
