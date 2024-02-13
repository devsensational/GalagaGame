using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class BezierUtil : MonoBehaviour
{
    //Inspector
    public GameObject StartObject;
    public GameObject EndObject;
    public GameObject PointObject;
    public GameObject MovementUnit;

    //JSON Inspector
    public string fileName = "bezier.json";

    //Time Inspector
    [Range(0f, 1f)]
    public float time = 0f;

    //CurveSmooth Inspector
    [Range(0, 10000)]
    public int CurveSmooth;

    //gizmos
    private List<Vector3> gizmosPoints = new List<Vector3>();

    //private
    private List<GameObject> pointObjects = new List<GameObject>();
    private List<Vector3> controlPoints = null;
    private BezierObject bezierPointer = null;

    private JObject bezierFile = null;
    private string filePath = "";

    private void Awake()
    {
        InitBezierUtil();
    }

    private void Update()
    {
        controlPoints[0] = StartObject.transform.position;
        controlPoints[controlPoints.Count - 1] = EndObject.transform.position;
        for (int i = 0; i < pointObjects.Count; i++) controlPoints[i + 1] = pointObjects[i].transform.position;

        MovementUnit.transform.position = CalculateBezierPoint(time, controlPoints);
    }

    private void OnDrawGizmos()
    {
        gizmosPoints.Clear();
        for (int i = 0; i < CurveSmooth; i++)
        {
            if(controlPoints != null)
            gizmosPoints.Add(CalculateBezierPoint((float)i / CurveSmooth, controlPoints));
        }
        for (int i = 0; i < gizmosPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(gizmosPoints[i], gizmosPoints[i + 1]);
        }
    }

    private void InitBezierUtil()
    {
        filePath = Path.Combine(Application.dataPath, "BezierUtility/Output", fileName);
        bezierPointer = new BezierObject();

        controlPoints = new List<Vector3>
        {
            StartObject.transform.position,
            EndObject.transform.position
        };

        CreateNewPointObject();

        ViewBezierObjects();
    }

    public void CreateNewPointObject()
    {
        Vector3 pos = new Vector3(0f, 0f, 0f);
        CreateNewPointObject(pos);
    }

    public void CreateNewPointObject(Vector3 pos)
    {
        GameObject ptr = Instantiate(PointObject);
        ptr.name = "P" + pointObjects.Count.ToString();
        ptr.transform.position = pos;
        pointObjects.Add(ptr);
        bezierPointer.PointList.Add(new float[3] { pos.x, pos.y, pos.z });
        controlPoints.Insert(controlPoints.Count - 1, MovementUnit.transform.position);
    }

    private void ViewBezierObjects()
    {
        Vector3 pos = new Vector3(0f, 0f, 0f);
        pos.Set(bezierPointer.StartPosition[0], bezierPointer.StartPosition[1], bezierPointer.StartPosition[2]);
        StartObject.transform.position = pos;
        pos.Set(bezierPointer.EndPosition[0], bezierPointer.EndPosition[1], bezierPointer.EndPosition[2]);
        EndObject.transform.position = pos;
        for (int i = 0; i < bezierPointer.PointList.Count; i++)
        {
            pos.Set(bezierPointer.PointList[i][0], bezierPointer.PointList[i][1], bezierPointer.PointList[i][2]);
            pointObjects[i].transform.position = pos;
        }
    }

    // ������ � ����� ���� �޼ҵ�
    public Vector3 CalculateBezierPoint(float t, List<Vector3> controlPoints)
    {
        int n = controlPoints.Count - 1; // �������� ������ ���� ���� n
        Vector3 point = Vector3.zero; // �ʱ�ȭ
        for (int i = 0; i <= n; i++)
        {
            // ���װ�� * (1-t)^(n-i) * t^i
            float binomialCoefficient = BinomialCoefficient(n, i);
            float term = binomialCoefficient * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i);
            point += term * controlPoints[i];
        }
        return point;
    }

    // ���װ��(����) ��� �޼ҵ�
    private float BinomialCoefficient(int n, int k)
    {
        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }

    // ���丮�� ��� �޼ҵ�
    private float Factorial(int n)
    {
        float result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    private void SyncPositionValue(float[] arr, Vector3 position)
    {
        arr[0] = position.x; arr[1] = position.y; arr[2] = position.z;
    }

    public void SaveBezierFile()
    {
        SyncPositionValue(bezierPointer.StartPosition, StartObject.transform.position);
        SyncPositionValue(bezierPointer.EndPosition, EndObject.transform.position);
        for(int i = 0; i < bezierPointer.PointList.Count; i++)
        {
            SyncPositionValue(bezierPointer.PointList[i], pointObjects[i].transform.position); 
        }

        // ��ü�� JSON ���ڿ��� ��ȯ
        string json = JsonConvert.SerializeObject(bezierPointer, Formatting.Indented);

        // ���Ͽ� JSON ���ڿ� ����
        File.WriteAllText(filePath, json);
        Debug.Log(json);
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
            bezierPointer = JsonConvert.DeserializeObject<BezierObject>(json);

            Vector3 pos = new Vector3();
            for(int i = 0; i < bezierPointer.PointList.Count; i++)
            {
                pos.Set(bezierPointer.PointList[i][0], bezierPointer.PointList[i][1], bezierPointer.PointList[i][2]);
                CreateNewPointObject(pos);
            }
            ViewBezierObjects();
            Debug.Log("Loaded Bezier file from " + filePath);
        }
        else
        {
            Debug.LogError("Cannot load Bezier file; file does not exist.");
        }
    }
}
