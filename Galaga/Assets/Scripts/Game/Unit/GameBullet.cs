using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class GameBullet : GameUnit, IGameBullet
{
    //Inspector
    [Header ("Bullet Inspector")]
    public float destroyTime;

    //protected
    protected Vector3             direction;

    //private
    private GameObjectPoolManager poolManager;

    public void ShootBullet(Vector3 direction)
    {
        this.direction = direction;
        Invoke("DestroyBullet", destroyTime);
    }

    public void ShootBullet(Vector3 direction, float destroyTime)
    {
        this.destroyTime = destroyTime;
        ShootBullet(direction);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(""))
        {
            DestroyBullet();
        }
    }
    public void UpdateBullet()
    {
        gameObject.transform.Translate(direction * moveSpeed * moveSpeedMultiplier * Time.deltaTime);
    }

    public void ChildUpdate() { }

    public void Update()
    {
        UpdateBullet();
        ChildUpdate();
    }

    public void Awake()
    {
        poolManager = GameObjectPoolManager.Instance;
    }

    public void DestroyBullet()
    {
        if (poolManager != null) 
        {
            poolManager.OnReleaseGameObject(GameUnitObjectType.BULLET, gameObject);
        }
    }
}
