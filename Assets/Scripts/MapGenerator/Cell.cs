using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private int x, y;
    private bool isTaken;
    private CellObjectType objectType;
    public int X { get => x; }
    public int Y { get => y; }
    public bool IsTaken { get => isTaken; set => isTaken = value; }
    public CellObjectType ObjectType { get => objectType; set => objectType = value; }

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        SetDefaultTiles();
        isTaken = false;
    }

    public void SetDefaultTiles()
    {
        float emptyChance = Random.Range(0, 10);
        if (emptyChance < 8.5f)
        {
            objectType = CellObjectType.Empty;
        }
        else
        {
            objectType = CellObjectType.Environment;
        }
        isTaken = true;
    }
}

public enum CellObjectType
{
    Environment,
    Resource,
    Empty
}
