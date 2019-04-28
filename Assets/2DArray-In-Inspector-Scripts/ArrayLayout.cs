using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArrayLayout  {

    public int rowSize;

	public RowData[] rows = new RowData[7]; //Grid of 7x7
}
[System.Serializable]
public struct RowData
{
    public int[] row;
}
