using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BezierObject
{
    public float[]          StartPosition   = null;
    public float[]          EndPosition     = null;
    public List<float[]>    PointList       = new List<float[]>();

    public BezierObject()
    {
        StartPosition = new float[3] { 1, 1, 1 };
        EndPosition = new float[3] { 2, 2, 2 };
    }
}
