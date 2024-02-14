using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitStatus: uint
{
    NONE        = 0x00,
    UNKNOWN     = 0x01,

    //아래에 Enum 값 작성
    DEFAULT     = 0x02,
    HIT         = 0x04,
    CAPUTRED    = 0x08,
    DAMAGED     = 0x10,
    DEAD        = 0x20,
    //Enum End

    End         = 0xFF
}
public class GameUnit : MonoBehaviour
{
    //Inspector
    [Header ("Common")]
    [SerializeField] protected int      Hp                  = 1;
    [SerializeField] protected float    moveSpeed           = 1f;
    [SerializeField] protected float    moveSpeedMultiplier = 1f;

    //Protected
    protected uint UnitStatusValue = (uint)UnitStatus.NONE;

    public void UnitMovement() { }
}
