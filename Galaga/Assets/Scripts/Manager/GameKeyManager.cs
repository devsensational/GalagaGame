using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyValues
{
    None    = 0,

    //아래에 Enum값 작성
    UP,
    DOWN, 
    LEFT, 
    RIGHT,
    FIRE,
    //Enum end

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
        KeyValuePairs.Add(KeyValues.FIRE, KeyCode.Z);
    }

    public void KeyChange(KeyValues keyValue, KeyCode keyCode)
    {
        KeyValuePairs[keyValue] = keyCode;
    }
}
