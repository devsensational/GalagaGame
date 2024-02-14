using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GamePlayerUnit : GameUnit, IGameUnitAttack, IGameUnitHit
{
    //Inspector
    [Header("Player Unit Inspector")]
    [SerializeField] private GameObject Bullet;

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            UnitHit();
        }
    }

    public void UnitAttack()
    {
        //Object Pool에서 꺼내올 것 
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

    void Awake()
    {

    }

    void Update()
    {

    }
}
