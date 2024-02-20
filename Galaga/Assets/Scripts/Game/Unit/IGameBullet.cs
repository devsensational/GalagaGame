using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameBullet
{
    abstract void ShootBullet(Vector3 direction);
    abstract void Update();
    abstract void DestroyBullet();
}
