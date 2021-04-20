using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapVisualizer : MonoBehaviour
{

    enum ResourceType
    {
        Wood, Stone, Gold
    }

    private float woodProbability = 0.5f;
    private float stoneProbability = 0.8f;
    //private float goldProbability = 1f;

    private Transform parent;

    public Color resourceColor, environmentColor;

    public GameObject[] woodResourcesNodes;
    public GameObject[] stoneResourcesNodes;
    public GameObject[] goldResourcesNodes;
    public GameObject[] environmentObjects;

    Dictionary<Vector3, GameObject> dictionaryOfResources = new Dictionary<Vector3, GameObject>();

    private void Awake()
    {
        parent = this.transform;
    }

    public void VisualizeMap(MapGrid grid, MapData data, bool visualizeUsingPrefabs)
    {
        if (visualizeUsingPrefabs)
        {
            VisualizeUsingPrefabs(grid, data);
        }
        else
        {
            VisualizeUsingPrimitives(grid, data);
        }
    }

    private void VisualizeUsingPrimitives(MapGrid grid, MapData data)
    {
        for (int i = 0; i < data.resourcesArray.Length; i++)
        {
            if (data.resourcesArray[i])
            {
                Vector3 positionOnGrid = grid.CalculateIndexFromCoordinatesFromIndex(i);

                if (IsNearToHQ(positionOnGrid, data.hqPosition)){
                    continue;
                }

                grid.SetCell(positionOnGrid.x, positionOnGrid.y, CellObjectType.Resource);

                if (PlaceKnightResource(data, positionOnGrid))
                {
                    continue;
                }

                if (dictionaryOfResources.ContainsKey(positionOnGrid) == false)
                {
                    CreateIndicator(positionOnGrid, Color.green, PrimitiveType.Sphere);
                }
            }
        }
    }

    private bool PlaceKnightResource(MapData data, Vector3 positionOnGrid)
    {
        foreach (KnightPiece knight in data.knightPiecesList)
        {
            if(knight.Position == positionOnGrid)
            {
                CreateIndicator(positionOnGrid, Color.red, PrimitiveType.Cube);
                return true;
            }
        }
        return false;
    }

    private void VisualizeUsingPrefabs(MapGrid grid, MapData data)
    {
        for (int col = 0; col < grid.Width; col++)
        {
            for (int row = 0; row < grid.Length; row++)
            {
                var cell = grid.GetCell(col, row);
                var position = new Vector3(cell.X, cell.Y, 0);

                var index = grid.CalculateIndexFromCoordinates(position.x, position.y);
                if (data.resourcesArray[index] && !grid.GetCell(cell.X, cell.Y).IsTaken)
                {
                    cell.ObjectType = CellObjectType.Resource;
                }

                switch (cell.ObjectType)
                {
                    case CellObjectType.Environment:
                        int randomIndex = Random.Range(0, environmentObjects.Length);
                        CreateIndicator(position, environmentObjects[randomIndex]);
                        break;
                    case CellObjectType.Resource:
                        GameObject resourceToSpawn = SelectRandomResourceNode();
                        CreateIndicator(position, resourceToSpawn);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private GameObject SelectRandomResourceNode()
    {
        ResourceType selectedResourceType = SelectRandomResourceType();

        if (selectedResourceType.Equals(ResourceType.Wood))
        {
            int randomIndex = Random.Range(0, woodResourcesNodes.Length);
            return woodResourcesNodes[randomIndex];
        }
        else if (selectedResourceType.Equals(ResourceType.Stone))
        {
            int randomIndex = Random.Range(0, stoneResourcesNodes.Length);
            return stoneResourcesNodes[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, goldResourcesNodes.Length);
            return goldResourcesNodes[randomIndex];
        }
    }

    private ResourceType SelectRandomResourceType()
    {
        ResourceType randomResourceType;

        float randomRange = Random.Range(0f, 1f);
        if(randomRange < woodProbability)
        {
            randomResourceType = ResourceType.Wood;
        }else if(randomRange >= woodProbability && randomRange < stoneProbability)
        {
            randomResourceType = ResourceType.Stone;
        }
        else
        {
            randomResourceType = ResourceType.Gold;
        }

        return randomResourceType;
    }

    private void CreateIndicator(Vector3 position, Color color, PrimitiveType primitiveType)
    {
        GameObject element = GameObject.CreatePrimitive(primitiveType);
        dictionaryOfResources.Add(position, element);
        element.transform.position = position;
        element.transform.parent = parent;

        Renderer renderer = element.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
    }
    private void CreateIndicator(Vector3 position, GameObject prefab, Quaternion rotation = new Quaternion())
    {
        var placementPosition = position; //+ new Vector3(.5f, .5f, .5f);
        var element = Instantiate(prefab, placementPosition, rotation);

        element.transform.parent = parent;
        dictionaryOfResources.Add(position, element);
    }

    public void ClearMap()
    {
        foreach (GameObject resource in dictionaryOfResources.Values)
        {
            Destroy(resource);
        }

        dictionaryOfResources.Clear();
    }
    private bool IsNearToHQ(Vector3 coordinates, Vector3 hqPosition)
    {
        return (coordinates.Equals(hqPosition) || coordinates.Equals(new Vector3(hqPosition.x, hqPosition.y - 1)) ||
            coordinates.Equals(new Vector3(hqPosition.x - 1, hqPosition.y)) || coordinates.Equals(new Vector3(hqPosition.x - 1, hqPosition.y - 1)) ||
            coordinates.Equals(new Vector3(hqPosition.x, hqPosition.y + 1)) || coordinates.Equals(new Vector3(hqPosition.x + 1, hqPosition.y)) ||
            coordinates.Equals(new Vector3(hqPosition.x + 1, hqPosition.y + 1)));
    }
}
