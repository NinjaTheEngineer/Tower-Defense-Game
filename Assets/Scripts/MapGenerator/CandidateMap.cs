using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandidateMap
{
    private MapGrid grid;
    private int numberOfPieces = 0;

    private Vector3 hqPosition;

    private int hqRadius = 3;

    private List<KnightPiece> knightPiecesList;

    private bool[] resourcesArray = null;

    public MapGrid Grid { get => grid; }
    public bool[] ResourcesArray { get => resourcesArray; }
    public Vector3 HqPosition { get => hqPosition;  }

    public CandidateMap(MapGrid grid, int numberOfPieces, Vector3 hqPosition, int hqRadius)
    {
        this.numberOfPieces = numberOfPieces;
        this.grid = grid;
        this.hqPosition = hqPosition;
        this.hqRadius = hqRadius;
    }

    public void CreateMap()
    {
        this.resourcesArray = new bool[grid.Width * grid.Length];
        this.knightPiecesList = new List<KnightPiece>();
        ShowWheresHQInCellPosition();
        RandomlyPlaceKnightPieces(this.numberOfPieces);

        PlaceResources();
    }

    public void ShowWheresHQInCellPosition()
    {
        Debug.Log("HQ POSITION == x_" + hqPosition.x + " -- y_" + hqPosition.y);
        GetHQCircleWithRadiusOf(hqRadius);
        grid.CheckCoordiantes();
    }

    private void GetHQCircleWithRadiusOf(int radius)
    {
        int numberOfLanes = radius * 2 + 1; //3         //5
        int endRow = (Mathf.FloorToInt(numberOfLanes / 2)); // 1        //2
        int startRow = -endRow; //-1        //-2

        int laneLenght = 1;
        int endLane = (Mathf.FloorToInt(numberOfLanes / 2)); // 1     //2
        int startLane = 0;
        int laneCount = 0;
        bool turned = false;
        for (int row = startRow; row < endRow + 1; row++) //3 vezes, -1, 0, 1       // 5vezes, -2, -1, 0, 1, -2
        {
                    //-1               //2         //3
            for (int lane = startLane ; laneCount < laneLenght; lane++) //3 vezes = 0, (-1 0 +1), 0    //5 vezes = 0, (-1 0 +1), (-2 -1 0 1 +2), (+1 0 -1), 0
            {
                if (laneLenght.Equals(1))
                {
                    grid.GetCell(HqPosition.x + row, HqPosition.y).SetDefaultTiles();
                }
                else
                {
                    grid.GetCell(HqPosition.x + row, HqPosition.y + lane).SetDefaultTiles();
                }
                laneCount++;
            }

            laneCount = 0;
            if (row < 0)
            {
                laneLenght += 2;

            }
            else
            {
                laneLenght += -2;
            }
            if(startLane > -endLane)
            {
                if (turned)
                {
                    startLane++;
                }
                else
                {
                    startLane--;
                }
            }
            else
            {
                turned = true;
                startLane++;
            }
        }
    }

    public MapData GetMapData()
    {
        return new MapData
        {
            resourcesArray = this.resourcesArray,
            knightPiecesList = this.knightPiecesList,
            hqPosition = this.HqPosition
        };
    }

    private void PlaceResources()
    {
        foreach (KnightPiece knight in knightPiecesList)
        {
            PlaceResourcesForThisKnight(knight);
        }
    }

    private void PlaceResourcesForThisKnight(KnightPiece knight)
    {
        foreach (Vector3 position in KnightPiece.listOfPossibleMoves)
        {
            var newPosition = knight.Position + position;

            if(grid.IsCellValid(newPosition.x, newPosition.y) && CheckIfPositionCanBeResource(newPosition))
            {
                resourcesArray[grid.CalculateIndexFromCoordinates(newPosition.x, newPosition.y)] = true;
            }
        }
    }

    private bool CheckIfPositionCanBeResource(Vector3 newPosition)
    {
        if (IsNearToHQ(newPosition))
        {
            return false;
        }

        int index = grid.CalculateIndexFromCoordinates(newPosition.x, newPosition.y);

        return resourcesArray[index] == false;
    }

    private void RandomlyPlaceKnightPieces(int numberOfPieces)
    {
        int count = numberOfPieces;
        int knightPlacementTryLimit = 100;

        while( count > 0 && knightPlacementTryLimit > 0)
        {
            int randomIndex = Random.Range(0, resourcesArray.Length);

            if(resourcesArray[randomIndex] == false)
            {
                Vector3 coordinates = grid.CalculateIndexFromCoordinatesFromIndex(randomIndex);
                
                if(grid.GetCell(coordinates.x, coordinates.y).IsTaken)
                {
                    continue;
                }
                else
                {
                    resourcesArray[randomIndex] = true;
                    knightPiecesList.Add(new KnightPiece(coordinates));
                    count--;
                }
            }
            knightPlacementTryLimit--;
        }
    }

    private bool IsNearToHQ(Vector3 coordinates)
    {
        return grid.GetCell(coordinates.x, coordinates.y).IsTaken;
        /*
        Collider2D[] objectsDetected = Physics2D.OverlapCircleAll(coordinates, 10);
        //Debug.Log("> size . " + objectsDetected.Length);
        if(objectsDetected.Length > 0)
        {
            foreach(Collider2D collider in objectsDetected)
            {
                if(collider.GetComponent<BuildingTypeHolder>().buildingType.isHQ())
                {
                    Debug.Log("Near HQ, change Position");
                    return true;
                }
            }
        }
        //Debug.Log("Far from HQ");
        return false;    

        /*return (coordinates.Equals(hqPosition) || coordinates.Equals(new Vector3(hqPosition.x, hqPosition.y - 1)) ||
            coordinates.Equals(new Vector3(hqPosition.x - 1, hqPosition.y)) || coordinates.Equals(new Vector3(hqPosition.x - 1, hqPosition.y - 1)) ||
            coordinates.Equals(new Vector3(hqPosition.x, hqPosition.y + 1)) || coordinates.Equals(new Vector3(hqPosition.x + 1, hqPosition.y )) ||
            coordinates.Equals(new Vector3(hqPosition.x + 1, hqPosition.y + 1)));
    */
    }
}
