using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameBullet
{
    abstract void ShootBullet(Vector3 direction);
    abstract public void ShootBullet(Vector3 direction, float destroyTime);
    abstract public void ShootBullet(Vector3 direction, GameUnitObjectType GUOType, string tag);
    abstract public void SetBulletParent(GameObject BulletParent);
    abstract void Update();
    abstract void DestroyBullet();
}
