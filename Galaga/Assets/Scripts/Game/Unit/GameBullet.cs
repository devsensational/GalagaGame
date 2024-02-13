using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameBullet : MonoBehaviour, IGameBullet
{
    //Inspector
    public float BulletSpeed;
    public float BulletSpeedMultiplier;

    //protected
    protected Vector3 direction;

    //private
    private IObjectPool<GameBullet> gameBulletPool;

    public GameBullet(Vector3 direction) {  this.direction = direction; }

    public void UpdateBullet(Vector3 direction)
    {

    }

    public void ChildUpdate() { }

    public void Update()
    {
        UpdateBullet(direction);
        ChildUpdate();
    }

    public void DestoryBullet()
    {
        if (gameBulletPool != null) 
        {
            gameBulletPool.Release(this);
        }
    }
}
