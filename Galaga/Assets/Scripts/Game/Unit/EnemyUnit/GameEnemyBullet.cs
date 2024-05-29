using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemyBullet : GameBullet
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerUnit")) { DestroyBullet(); }
        if (collision.gameObject.CompareTag("Outside")) { DestroyBullet(); }
    }
    override public void DestroyBullet()
    {
        if (poolManager != null || gameObject.activeSelf == true)
        {
            poolManager.OnReleaseGameObject(type, gameObject);
        }
    }
    override protected void ChildUpdate() { }
    override protected void ChildAwake()
    {
        type = GameUnitObjectType.ENEMYBULLET;
    }
}
