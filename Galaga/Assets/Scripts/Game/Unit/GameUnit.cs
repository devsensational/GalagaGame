using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour, IGameAttackable
{
    [SerializeField] protected int Hp = 1;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveSpeedMultiplier = 1f;

    public bool UnitAttack()
    {
        throw new System.NotImplementedException();
    }

    public bool UnitDamaged()
    {
        return true;
    }

    private void UnitDisable()
    {
        gameObject.SetActive(false);
    }
}
