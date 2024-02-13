using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyValues
{
    None    = 0,

    //여기서 부터 작성
    UP,
    DOWN, 
    LEFT, 
    RIGHT,
    FIRE,
    //

    End = 300
}
public class GameKeyManager : MonoSingleton<GameKeyManager>
{
    //public
    public Dictionary<KeyValues, KeyCode> KeyValuePairs { get; set; }

    protected override void ChildAwake()
    {
        Init();
    }

    protected override void ChildOnDestroy()
    {

    }

    private void Init()
    {
        KeyValuePairs = new Dictionary<KeyValues, KeyCode>();

        KeyValuePairs.Add(KeyValues.LEFT, KeyCode.LeftArrow);
        KeyValuePairs.Add(KeyValues.RIGHT, KeyCode.RightArrow);
        KeyValuePairs.Add(KeyValues.RIGHT, KeyCode.Z);
    }

    private KeyCode KeyDetect()
    {
        return KeyCode.None;
    }
}
