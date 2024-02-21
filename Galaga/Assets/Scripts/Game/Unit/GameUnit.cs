using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    //Inspector
    [Header ("Common")]
    [SerializeField] protected int      Hp                  = 1;
    [SerializeField] protected float    moveSpeed           = 1f;
    [SerializeField] protected float    moveSpeedMultiplier = 1f;

}
