using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVUtil
{
    private (byte, byte)[,] byteData;

    public (byte, byte)[,] ReadCSV(TextAsset csv)
    {
        // ������ ������ �� ������ �и��մϴ�.
        string[] rows = csv.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // ù ��° ���� �������� ���� ���� �����մϴ�.
        int cols = rows[0].Split(',').Length;
        (byte, byte)[,] byteData = new (byte, byte)[rows.Length, cols];

        for (int i = 0; i < rows.Length; i++)
        {
            // �� ���� �޸��� �и��Ͽ� ���� �����մϴ�.
            string[] cells = rows[i].Split(',');
            for (int j = 0; j < cells.Length; j++)
            {
                // �� ���� �����ݷ����� �и��Ͽ� (byte, byte) Ʃ�÷� ��ȯ�մϴ�.
                string[] parts = cells[j].Split(';');
                if (parts.Length == 2 && byte.TryParse(parts[0], out byte part1) && byte.TryParse(parts[1], out byte part2))
                {
                    byteData[i, j] = (part1, part2);
                }
                else
                {
                    Debug.LogError($"��ȯ ����: �� {i + 1}, �� {j + 1} ('{cells[j]}')");
                    return null; // ��ȯ�� ������ ���, null�� ��ȯ�� �� �ֽ��ϴ�.
                }
            }
        }

        return byteData;
    }
}
