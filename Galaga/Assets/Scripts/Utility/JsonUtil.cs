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

    public T LoadBezierFile<T>()
    {
        T objectPointer = default;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            objectPointer = JsonConvert.DeserializeObject<T>(json);
            Debug.Log("Loaded Json file from " + filePath);
        }
        else
        {
            Debug.LogError("Cannot load Bezier file; file does not exist.");
        }
        return objectPointer;
    }
}
