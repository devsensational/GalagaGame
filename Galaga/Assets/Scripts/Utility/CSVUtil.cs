using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVUtil
{
    private (byte, byte)[,] byteData;

    public (byte, byte)[,] ReadCSV(TextAsset csv)
    {
        // 파일의 내용을 줄 단위로 분리합니다.
        string[] rows = csv.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // 첫 번째 줄을 기준으로 열의 수를 결정합니다.
        int cols = rows[0].Split(',').Length;
        (byte, byte)[,] byteData = new (byte, byte)[rows.Length, cols];

        for (int i = 0; i < rows.Length; i++)
        {
            // 각 줄을 콤마로 분리하여 셀을 추출합니다.
            string[] cells = rows[i].Split(',');
            for (int j = 0; j < cells.Length; j++)
            {
                // 각 셀을 세미콜론으로 분리하여 (byte, byte) 튜플로 변환합니다.
                string[] parts = cells[j].Split(';');
                if (parts.Length == 2 && byte.TryParse(parts[0], out byte part1) && byte.TryParse(parts[1], out byte part2))
                {
                    byteData[i, j] = (part1, part2);
                }
                else
                {
                    Debug.LogError($"변환 실패: 줄 {i + 1}, 셀 {j + 1} ('{cells[j]}')");
                    return null; // 변환에 실패한 경우, null을 반환할 수 있습니다.
                }
            }
        }

        return byteData;
    }
}
