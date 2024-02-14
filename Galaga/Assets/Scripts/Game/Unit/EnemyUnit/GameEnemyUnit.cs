using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameEnemyUnit : GameUnit, IGameUnitAttack, IGameUnitHit
{
    //Inspector Field
    [Header("Enemy Unit Inspector")]
    [SerializeField] private int        Score;
    [SerializeField] private TextAsset  PattenFile;
    [SerializeField] private GameObject Bullet;
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            UnitHit();
        }
        else if (collision.gameObject.CompareTag("PlayerUnit"))
        {
            UnitHit();
        }
    }

    public void UnitAttack()
    {

    }

    public void UnitHit()
    { 

    }

    void Awake() { }
    void Update() {}

}
