using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitStatus: uint
{
    None        = 0x00,
    Unknown     = 0x01,

    //아래에 Enum 값 작성
    Default     = 0x02,
    Hit         = 0x04,
    Caputred    = 0x08,
    Dead        = 0x10,
    //Enum End

    End         = 0xFF
}
public class GameUnit : MonoBehaviour
{
    //Inspector
    [SerializeField] protected int      Hp                  = 1;
    [SerializeField] protected float    moveSpeed           = 1f;
    [SerializeField] protected float    moveSpeedMultiplier = 1f;

    //Protected
    protected uint UnitStatusValue = (uint)UnitStatus.None;

    public void UnitMovement() { }
}
