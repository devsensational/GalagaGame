using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePathGenerator
{
    public List<Vector3> CalculateBezierPathPoints(int pointCount, List<Vector3> controlPoints, float threshold = 0.9f)
    {
        List<Vector3> pathPoints = new List<Vector3>();
        float step = 1.0f / (pointCount - 1); // ���� ������ ����

        Vector3 lastPoint = controlPoints[controlPoints.Count - 1]; // ������ ��
        Vector3 secondLastPoint = CalculateBezierPoint(threshold, controlPoints); // ��ȯ ����

        for (int i = 0; i < pointCount; i++)
        {
            float t = i * step; // ���� t �Ű����� ��
            Vector3 point;

            if (t <= threshold)
            {
                // � �κ��� ����Ʈ ���
                point = CalculateBezierPoint(t / threshold, controlPoints); // ������ � ���� �� ���
            }
            else
            {
                // threshold ���Ĵ� ��� ����Ʈ�� �������� ����
                point = lastPoint; // �������� ���� �̵�
            }

            pathPoints.Add(point); // ���� ���� ����Ʈ�� �߰�
        }

        return pathPoints;
    }

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

    private float BinomialCoefficient(int n, int k)
    {
        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }

    private float Factorial(int n)
    {
        float result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

}
