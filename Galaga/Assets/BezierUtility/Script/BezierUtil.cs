using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BezierUtil : MonoBehaviour
{
    private List<BezierObject> bezierObjects = new List<BezierObject>();
    private BezierObject bezierPointer = null;

    //JSON
    private JObject bezierFile = null;
    private string fileName = "bezier.json";
    private string filePath = "";

    private void Awake()
    {
        filePath = Path.Combine(Application.dataPath, "BezierUtility/Output", fileName);
        InitBezierUtil();
    }

    private void InitBezierUtil()
    {
        bezierObjects = new List<BezierObject>();
        temp(0);
        temp(1);
        temp(2);
        temp(3);
    }

    void temp(int idx)
    {
        bezierObjects.Add(new BezierObject());
        bezierObjects[idx].StartPosition = new float[3] { 1, 1, 1 };
        bezierObjects[idx].EndPosition = new float[3] { 1, 1, 1 };
        bezierObjects[idx].PointList.Add(new float[3] { 1, 1, 1 });
        bezierObjects[idx].PointList.Add(new float[3] { 2, 2, 2 });
        bezierObjects[idx].PointList.Add(new float[3] { 3, 3, 31 });
    }
    public void SaveBezierFile()
    {
        // 객체를 JSON 문자열로 변환
        string json = JsonConvert.SerializeObject(bezierObjects, Formatting.Indented);

        // 파일에 JSON 문자열 쓰기
        File.WriteAllText(filePath, json);
        Debug.Log("Saved Bezier file to " + filePath);
    }

    public void LoadBezierFile()
    {
        // 파일이 존재하는지 확인
        if (File.Exists(filePath))
        {
            // 파일에서 JSON 문자열 읽기
            string json = File.ReadAllText(filePath);

            // JSON 문자열을 객체로 변환
            bezierObjects = JsonConvert.DeserializeObject<List<BezierObject>>(json);
            Debug.Log("Loaded Bezier file from " + filePath);
        }
        else
        {
            Debug.LogError("Cannot load Bezier file; file does not exist.");
        }
    }

}
