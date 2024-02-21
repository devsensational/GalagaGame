using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class GamePlayerBullet : GameBullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerUnit") && gameObject.CompareTag("EnemyBullet")) { DestroyBullet(); }
        if (collision.gameObject.CompareTag("EnemyUnit") && gameObject.CompareTag("PlayerBullet")) { DestroyBullet(); }
    }
    override public void DestroyBullet()
    {
        if (poolManager != null)
        {
            //Bullet이 사라질 때 코드를 해당 주석 사이에 작성해야 함
            BulletParent.GetComponent<GamePlayerUnit>().BulletCount--;
            //
            poolManager.OnReleaseGameObject(type, gameObject);
        }
    }

    override protected void ChildUpdate() { }
    protected override void ChildAwake(){ }
}
