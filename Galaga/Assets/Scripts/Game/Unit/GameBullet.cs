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
    protected Vector3               direction;
    protected GameObjectPoolManager poolManager;
    protected GameObject            BulletParent;
    protected GameUnitObjectType    type;
    //private

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

    public void ShootBullet(Vector3 direction, GameUnitObjectType GUOType,string tag)
    {
        gameObject.tag = tag;
        type = GUOType;
        ShootBullet(direction);
    }

    public void SetBulletParent(GameObject BulletParent)
    {
        this.BulletParent = BulletParent;
    }

    public void UpdateBullet()
    {
        gameObject.transform.Translate(direction * moveSpeed * moveSpeedMultiplier * Time.deltaTime);
    }

    public void Update()
    {
        UpdateBullet();
        ChildUpdate();
    }

    public void Awake()
    {
        poolManager = GameObjectPoolManager.Instance;
        ChildAwake();
    }

    virtual public void DestroyBullet()
    {
        if (poolManager != null) 
        {
            //Bullet�� ����� �� �ڵ带 �ش� �ּ� ���̿� �ۼ��ؾ� ��

            //
            poolManager.OnReleaseGameObject(type, gameObject);
        }
    }
    virtual protected void ChildUpdate() { }
    virtual protected void ChildAwake() { }

}
