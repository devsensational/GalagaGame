using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

[CustomEditor(typeof(BezierUtil))]
public class BezierGenerateButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        BezierUtil bezierGenerateButton = (BezierUtil)target;
        if (GUILayout.Button("Create new point"))
        {
            bezierGenerateButton.CreateNewPointObject();
        }
        if (GUILayout.Button("Save Bezier File"))
        {
            bezierGenerateButton.SaveBezierFile();
        }
        if(GUILayout.Button("Load Bezier File"))
        {
            bezierGenerateButton.LoadBezierFile();
        }
    }
}
