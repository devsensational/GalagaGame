using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GamePlayerUnit : GameUnit, IGameUnitAttack, IGameUnitHit
{
    //Inspector
    [Header("Player Unit Inspector")]
    [SerializeField] private GameObject Bullet;

    //private
    GameObjectPoolManager poolManager;
    GameObject bulletPtr;

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            UnitHit();
        }
    }

    public void UnitAttack()
    {
        bulletPtr = poolManager.OnGetGameObject(GameUnitObjectType.BULLET);
        bulletPtr.transform.position = gameObject.transform.position;
        bulletPtr.SetActive(true);
        bulletPtr.GetComponent<GameBullet>().ShootBullet(Vector3.up);
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
        poolManager = GameObjectPoolManager.Instance;
        poolManager.CreateGameObjectPool(GameUnitObjectType.BULLET, Bullet, 3);
    }

    void Update()
    {

    }
}
