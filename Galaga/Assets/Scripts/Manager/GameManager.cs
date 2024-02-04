using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    int Score = 0;
    int StageLevel = 0;

    protected override void ChildAwake()
    {

    }

    protected override void ChildOnDestroy()
    {

    }
}
