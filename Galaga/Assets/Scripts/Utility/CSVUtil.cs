using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVUtil
{
    private byte[,] byteData;

    public byte[,] ReadCSV(TextAsset csv)
    {
        string[] rows = csv.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        int cols = rows[0].Split(',').Length;
        byteData = new byte[rows.Length, cols];

        for (int i = 0; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(',');
            for (int j = 0; j < cells.Length; j++)
            {
                byte value;
                if (byte.TryParse(cells[j], out value))
                {
                    byteData[i, j] = value;
                }
                else
                {
                    Debug.Log("Parse failed: " + cells[j] + j);
                }
            }
        }
        return byteData;
    }
}
