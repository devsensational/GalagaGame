using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum GameUnitObjectType
{
    NONE = 0,

    //아래에 Enum값 작성
    UNIT        = 1,
    BULLET      = 2,
    PLAYERUNIT  = 3,
    ENEMYUNIT   = 4,
    //Enum end

    END = 99
}

public class GameObjectPoolManager : MonoSingleton<GameObjectPoolManager>
{
    public Dictionary<GameUnitObjectType, List<GameUnit>> ObjectPool { get; set; }


    protected override void ChildAwake()
    {

    }

    protected override void ChildOnDestroy()
    {

    }
}
