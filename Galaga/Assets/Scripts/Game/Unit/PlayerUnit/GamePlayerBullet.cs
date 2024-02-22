using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class GamePlayerBullet : GameBullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyUnit")) { DestroyBullet(); }
        if (collision.gameObject.CompareTag("Outside")) { DestroyBullet(); }
    }
    override public void DestroyBullet()
    {
        if (poolManager != null || gameObject.activeSelf == true)
        {
            //Bullet이 사라질 때 코드를 해당 주석 사이에 작성해야 함
            BulletParent.GetComponent<GamePlayerUnit>().SubUnitBulletCount();
            //
            poolManager.OnReleaseGameObject(type, gameObject);
        }
    }

    override protected void ChildUpdate() { }
    override protected void ChildAwake()
    {
        type = GameUnitObjectType.PLAYERBULLET;
    }
}
