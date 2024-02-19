using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileUtilityManager : MonoSingleton<FileUtilityManager>
{
    //public 
    public JsonUtil JsonUtil    { get; private set; }
    public CSVUtil  CSVUtil     { get; private set; }

    protected override void ChildAwake()
    {
        JsonUtil    = new JsonUtil();
        CSVUtil     = new CSVUtil();
    }

    protected override void ChildOnDestroy()
    {

    }
}
