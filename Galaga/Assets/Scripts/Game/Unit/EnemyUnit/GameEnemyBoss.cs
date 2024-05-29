using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemyBoss : GameEnemyUnit
{
    private int hitCount = 0;

    public override void UnitHit()
    {
        base.UnitHit();
        hitCount++;
        if (hitCount * 2 <= SpriteList.Count) { spriteRenderer.sprite = SpriteList[(hitCount * 2) + (spriteIdx++ % 2)]; }
    }

    protected override IEnumerator OnAnimationSwitch()
    {
        if (SpriteList == null)
        {
            yield break;
        }
        if (hitCount * 2 <= SpriteList.Count) { spriteRenderer.sprite = SpriteList[(hitCount * 2) + (spriteIdx++ % 2)]; }

        yield return spriteChangeWaitForSeconds;
        StartCoroutine(OnAnimationSwitch());
    }

    public void StartPattern()
    {

    }

}
