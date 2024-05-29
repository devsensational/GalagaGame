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
    PLAYERUNIT,
    PLAYERBULLET,
    ENEMYUNIT,
    ENEMYBULLET,
    //Enum end

    END = 99
}

public class GameObjectPoolManager : MonoSingleton<GameObjectPoolManager>
{

    //public
    public Dictionary<GameUnitObjectType, List<GameObject>> ObjectPool { get; private set; }
    public Dictionary<GameUnitObjectType, GameObject>       ObjectPoolType { get; private set; }
    public Dictionary<GameUnitObjectType, int>              ObjectPoolMax { get; private set; }

    public GameObject OnGetGameObject(GameUnitObjectType GUOType)
    {
        if (!ObjectPool.ContainsKey(GUOType)) 
        { 
            Debug.Log("This ObjectPool has empty"); 
            return null; 
        }
        if (ObjectPool[GUOType].Count <= 0) { return NewCreateGameObject(GUOType); }

        GameObject ptr;
        ptr = ObjectPool[GUOType][ObjectPool.Count - 1];
        ObjectPool[GUOType].RemoveAt(ObjectPool.Count - 1);
        return ptr;
    }

    public void OnReleaseGameObject(GameUnitObjectType GUOType, GameObject gameObject)
    {
        if (!ObjectPool.ContainsKey(GUOType)) 
        { 
            Debug.Log("This ObjectPool has empty"); 
            return; 
        }
        if (ObjectPool[GUOType].Count > ObjectPoolMax[GUOType]) 
        { 
            Destroy(gameObject); 
            return; 
        }
        ObjectPool[GUOType].Add(gameObject);
        gameObject.SetActive(false);
    }

    public void CreateGameObjectPool(GameUnitObjectType GUOType, GameObject gameObject, int maxValue)
    {
        if (ObjectPool.ContainsKey(GUOType))
        {
            Debug.Log("Already created this objectpool");  
            return; 
        }
        else
        {
            ObjectPool.     Add(GUOType, new List<GameObject>());
            ObjectPoolMax.  Add(GUOType, maxValue);
            ObjectPoolType. Add(GUOType, gameObject);
            GameObject ptr;
            for (int i = 0; i < ObjectPoolMax[GUOType]; i++)
            {
                ptr = NewCreateGameObject(GUOType);
                ObjectPool[GUOType].Add(ptr);
                ptr.SetActive(false);
            }
            Debug.Log("Create ObjectPool: " + GUOType.ToString());
        }
    }

    private GameObject NewCreateGameObject(GameUnitObjectType GUOType)
    {
        GameObject ptr;
        ptr = Instantiate(ObjectPoolType[GUOType]);

        return ptr;
    }

    public void Init()
    {
        ObjectPool      = new Dictionary<GameUnitObjectType, List<GameObject>>();
        ObjectPoolType  = new Dictionary<GameUnitObjectType, GameObject>();
        ObjectPoolMax   = new Dictionary<GameUnitObjectType, int>();
    }

    protected override void ChildAwake()
    {
        DontDestroyOnLoad(this);
        Init();
    }

    protected override void ChildOnDestroy()
    {

    }
}
