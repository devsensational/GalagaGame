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
        // ��ü�� JSON ���ڿ��� ��ȯ
        string json = JsonConvert.SerializeObject(bezierObjects, Formatting.Indented);

        // ���Ͽ� JSON ���ڿ� ����
        File.WriteAllText(filePath, json);
        Debug.Log("Saved Bezier file to " + filePath);
    }

    public void LoadBezierFile()
    {
        // ������ �����ϴ��� Ȯ��
        if (File.Exists(filePath))
        {
            // ���Ͽ��� JSON ���ڿ� �б�
            string json = File.ReadAllText(filePath);

            // JSON ���ڿ��� ��ü�� ��ȯ
            bezierObjects = JsonConvert.DeserializeObject<List<BezierObject>>(json);
            Debug.Log("Loaded Bezier file from " + filePath);
        }
        else
        {
            Debug.LogError("Cannot load Bezier file; file does not exist.");
        }
    }

}
