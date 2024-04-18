using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePathGenerator
{
    public List<Vector3> CalculateBezierPathPoints(int pointCount, List<Vector3> controlPoints, float threshold = 0.9f)
    {
        List<Vector3> pathPoints = new List<Vector3>();
        float step = 1.0f / (pointCount - 1); // 점들 사이의 간격

        Vector3 lastPoint = controlPoints[controlPoints.Count - 1]; // 목적지 점
        Vector3 secondLastPoint = CalculateBezierPoint(threshold, controlPoints); // 변환 지점

        for (int i = 0; i < pointCount; i++)
        {
            float t = i * step; // 현재 t 매개변수 값
            Vector3 point;

            if (t <= threshold)
            {
                // 곡선 부분의 포인트 계산
                point = CalculateBezierPoint(t / threshold, controlPoints); // 베지에 곡선 상의 점 계산
            }
            else
            {
                // threshold 이후는 모든 포인트를 목적지로 설정
                point = lastPoint; // 목적지로 직접 이동
            }

            pathPoints.Add(point); // 계산된 점을 리스트에 추가
        }

        return pathPoints;
    }

    public Vector3 CalculateBezierPoint(float t, List<Vector3> controlPoints)
    {
        int n = controlPoints.Count - 1; // 제어점의 개수에 따른 차수 n
        Vector3 point = Vector3.zero; // 초기화
        for (int i = 0; i <= n; i++)
        {
            // 이항계수 * (1-t)^(n-i) * t^i
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
