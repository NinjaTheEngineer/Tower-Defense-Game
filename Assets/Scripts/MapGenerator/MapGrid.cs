using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class MapGrid
{
    private int width, length;
    private Cell[,] cellGrid;

    public int Width { get => width; }
    public int Length { get => length; }

    public MapGrid(int width, int length)
    {
        this.width = width;
        this.length = length;
        CreateGrid();
    }
    private void CreateGrid()
    {
        cellGrid = new Cell[length, width];

        for (int row = 0; row < length; row++)
        {
            for (int col = 0; col < width; col++)
            {
                cellGrid[row, col] = new Cell(col, row);
            }
        }
    }

    public void SetCell(int x, int y, CellObjectType objectType, bool isTaken = false)
    {
        cellGrid[y, x].ObjectType = objectType;
        cellGrid[y, x].IsTaken = isTaken;
    }

    public void SetCell(float x, float y, CellObjectType objectType, bool isTaken = false)
    {
        SetCell((int)x, (int)y, objectType, isTaken);
    }

    public bool IsCellTaken(int x, int y)
    {
        return cellGrid[y, x].IsTaken;
    }
    public bool IsCellTaken(float x, float y)
    {
        return cellGrid[(int)y, (int)x].IsTaken;
    }

    public Vector3 CalculateIndexFromCoordinatesFromIndex(int randomIndex)
    {
        int x = randomIndex % width;
        int y = randomIndex / width;

        return new Vector3(x, y, 0);
    }

    public bool IsCellValid(float x, float y)
    {
        if(IsCellOutOfGrid(x, y))
        {
            return false;
        }

        return true;
    }
    public Cell GetCell(int x, int y)
    {
        if(!IsCellValid(x, y))
        {
            return null;
        }
        return cellGrid[y, x];
    }

    public Cell GetCell(float x, float y)
    {
        return GetCell((int)x, (int)y);
    }

    public int CalculateIndexFromCoordinates(int x, int y)
    {
        return x + y * width;
    }

    public int CalculateIndexFromCoordinates(float x, float y)
    {
        return CalculateIndexFromCoordinates((int)x, (int)y);
    }

    public void CheckCoordiantes()
    {
        for (int i = 0; i < cellGrid.GetLength(0); i++)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int j = 0; j < cellGrid.GetLength(1); j++)
            {
                //stringBuilder.Append("(" + j + ", " + i + ") == "+ cellGrid[j, i].IsTaken.ToString() + ", ");
                string state;
                if(cellGrid[j, i].IsTaken)
                {
                    state = "T";
                }
                else
                {
                    state = "F";
                }
                stringBuilder.Append("[" + state + "] || ");
            }
            //Debug.Log(stringBuilder.ToString());
        }
    }

    private bool IsCellOutOfGrid(float x, float y)
    {
        return (x >= width || x < 0 || y >= length || y < 0);
    }
}
