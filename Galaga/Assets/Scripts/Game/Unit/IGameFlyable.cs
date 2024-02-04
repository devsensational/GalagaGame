using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameFlyable
{
    abstract void EnableUnit();
    abstract void DisableUnit();
    abstract void FlyUnit();
}
