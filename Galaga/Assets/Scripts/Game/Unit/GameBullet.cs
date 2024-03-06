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

    //public
    public bool  IsHit {  get; set; }

    //protected
    protected Vector3               direction;
    protected GameObjectPoolManager poolManager;
    protected GameObject            BulletParent;
    protected GameUnitObjectType    type;

    //private

    public void ShootBullet(Vector3 direction)
    {
        this.direction = direction;
    }

    public void ShootBullet(Vector3 direction, float destroyTime)
    {
        this.destroyTime = destroyTime;
        ShootBullet(direction);
    }

    public void SetBulletParent(GameObject BulletParent)
    {
        this.BulletParent = BulletParent;
    }

    private void UpdateBullet()
    {
        gameObject.transform.Translate(direction * moveSpeed * moveSpeedMultiplier * Time.deltaTime);
    }

    virtual public void DestroyBullet()
    {
        if (poolManager != null || gameObject.activeSelf == false)
        {
            //Bullet이 사라질 때 코드를 해당 주석 사이에 작성해야 함

            //
            poolManager.OnReleaseGameObject(type, gameObject);
        }
    }

    public void Update()
    {
        UpdateBullet();
        ChildUpdate();
    }

    public void Awake()
    {
        poolManager = GameObjectPoolManager.Instance;
        IsHit       = false;
        ChildAwake();
    }

    virtual protected void ChildUpdate() { }
    virtual protected void ChildAwake() { }

}
