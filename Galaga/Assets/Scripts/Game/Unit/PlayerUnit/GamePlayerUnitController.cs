using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerUnitController : MonoBehaviour
{
    //private
    Dictionary<KeyValues, KeyCode> keyValuePairs;
    GamePlayerUnit playerUnit;

    private void Awake()
    {
        keyValuePairs = GameKeyManager.Instance.KeyValuePairs;
        playerUnit = gameObject.GetComponent <GamePlayerUnit>() ;
    }

    private void Update()
    {
        if (Input.GetKey(keyValuePairs[KeyValues.LEFT]))
        {
            playerUnit.UnitMoveControl(Vector3.left);
        } 
        if (Input.GetKey(keyValuePairs[KeyValues.RIGHT]))
        {
            playerUnit.UnitMoveControl(Vector3.right);
        }
        if (Input.GetKeyDown(keyValuePairs[KeyValues.FIRE]))
        {
            playerUnit.UnitAttack();
        }
    }
}
