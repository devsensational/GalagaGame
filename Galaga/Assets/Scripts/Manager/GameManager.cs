using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoSingleton<GameManager>
{
    //
    private int Score { get; set; }
    private int StageLevel { get; set; }

    //ObjectPool
    private Dictionary<string, IObjectPool<GameObject>> ObjectPool { get; set; }

    protected override void ChildAwake()
    {
        DontDestroyOnLoad(this.gameObject);
        Init();
    }

    protected override void ChildOnDestroy()
    {

    }

    private void Init()
    {
        ObjectPool = new Dictionary<string, IObjectPool<GameObject>> { };
    }
}
