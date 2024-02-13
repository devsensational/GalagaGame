using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameUnitHit
{
    abstract void UnitHit();
    abstract void OnCollisionEnter(Collision collision);
}
