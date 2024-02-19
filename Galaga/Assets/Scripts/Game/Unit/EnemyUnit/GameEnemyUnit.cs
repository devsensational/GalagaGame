using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyUnitStatus : uint
{
    NONE = 0x00,
    UNKNOWN = 0x01,

    //아래에 Enum 값 작성
    DEFAULT = 0x02,
    HIT = 0x04,
    CAPUTRED = 0x08,
    DAMAGED = 0x10,
    MOVE = 0x20,
    DEAD = 0x30,
    //Enum End

    End = 0xFF
}

public class GameEnemyUnit : GameUnit, IGameUnitAttack, IGameUnitHit
{
    //Inspector Field
    [Header("Enemy Unit Inspector")]
    [SerializeField] private int                Score;
    [SerializeField] private List<TextAsset>    PattenFile;
    [SerializeField] private GameObject         Bullet;

    //public
    int GridPosition { get; set; }

    //private
    GameObject PlayerUnit;


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
        Hp -= 1;
    }

    private Vector3 AimPlayerUnit()
    {
        if (PlayerUnit != null) { return Vector3.zero; }
        Vector3 direction = gameObject.transform.position - PlayerUnit.transform.position;
        return direction.normalized;
    }

    void Awake() 
    {
        PlayerUnit = GameObject.Find("SpaceShip");
    }

    void Update() 
    {
        AimPlayerUnit();
    }

}
