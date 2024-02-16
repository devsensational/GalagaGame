using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameBullet
{
    abstract void OnCollisionEnter(Collision collision);
    abstract void ShootBullet(Vector3 direction);
    abstract void Update();
    abstract void DestroyBullet();
}
