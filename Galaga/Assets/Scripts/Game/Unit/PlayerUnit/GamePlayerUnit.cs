using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GamePlayerUnit : GameUnit, IGameUnitAttack, IGameUnitHit
{
    //ObjectPool
    private IObjectPool<IGameBullet> GameBulletObjectPool;
    


    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("bullet"))
        {
            UnitHit();
        }
    }

    public void UnitAttack()
    {
        Debug.Log("player attack");
    }

    public void UnitHit()
    {
        Hp -= 1;
        CheckPlayerDead();
    }

    private bool CheckPlayerDead()
    {
        if (Hp <= 0) return true; return false;
    }

    public void UnitMoveControl(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * moveSpeedMultiplier * Time.deltaTime);
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}
