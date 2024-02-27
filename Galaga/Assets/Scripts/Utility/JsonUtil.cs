using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonUtil
{
    //JSON Inspector
    public string fileName = "json.json";
    public string filePath = "";

    public void SaveBezierFile<T>(T objectPointer)
    {
        string json = JsonConvert.SerializeObject(objectPointer, Formatting.Indented);

        File.WriteAllText(filePath, json);
        Debug.Log("Saved Json file to " + filePath);
    }

    public T LoadBezierFile<T>(TextAsset textAsset)
    { 
        T objectPointer = default;

        if (textAsset != null)
        {
            try
            {
                string json = textAsset.text;
                objectPointer = JsonConvert.DeserializeObject<T>(json);
                Debug.Log("Loaded Json file from TextAsset");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error deserializing JSON from TextAsset: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Cannot load Bezier file; TextAsset is null.");
        }

        return objectPointer;
    }
}
