using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameEnemyUnit : GameUnit, IGameUnitAttack, IGameUnitHit
{
    //Inspector Field
    [SerializeField] private int Score;
    [SerializeField] private TextAsset PattenFile;

    public void OnCollisionEnter(Collision collision)
    {
        throw new System.NotImplementedException();
    }

    public void UnitAttack()
    {
        throw new System.NotImplementedException();
    }

    public void UnitHit()
    {
        throw new System.NotImplementedException();
    }

    void Awake() { }
    void Update() {}

}
