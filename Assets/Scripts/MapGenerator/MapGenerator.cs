using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GridVisualizer gridVisualizer;
    private CandidateMap map;
    public MapVisualizer mapVisualizer;
    public Transform cameraMain;
    public GameObject miniMapCamera;

    public PolygonCollider2D cameraBoundsCollider;

    private Vector3 centerPosition;
    public Transform hqGameObject;
    public bool randomPlacement;

    public bool visualizeUsingPrefabs = false;

    [Range(1, 100)]
    public int numberOfPieces = 5;

    [Range(1, 10)]
    public int hqRadius = 3;

    [Range(3, 500)]
    public int width, length = 10;

    private MapGrid grid;

    private void Start()
    {
        gridVisualizer.VisualizeGrid(width, length);
        GenerateNewMap();
    }

    private void GenerateNewMap()
    {
        int hqRadiusExpanded = hqRadius * 5;
        centerPosition = new Vector2(width / 2, length / 2);

        SetUpEverything(centerPosition);

        mapVisualizer.ClearMap();
        grid = new MapGrid(width, length);
        map = new CandidateMap(grid, numberOfPieces, centerPosition, hqRadiusExpanded);
        map.CreateMap();
        mapVisualizer.VisualizeMap(grid, map.GetMapData(), visualizeUsingPrefabs);
    }

    private void SetUpEverything(Vector3 centerPosition)
    {
        EnemyWaveManager.Instance.SetSpawnPositionList(centerPosition, width / 1.2f, length / 1.2f);
        hqGameObject.position = centerPosition;

        miniMapCamera.GetComponent<Camera>().orthographicSize = (width / 2) + 1;
        miniMapCamera.GetComponent<Transform>().position = centerPosition;

        cameraBoundsCollider.points = new Vector2[] { new Vector2(0, 0), new Vector2(width, 0), new Vector2(width, length), new Vector2(0, length) };
        cameraMain.position = new Vector3(centerPosition.x, centerPosition.y, cameraMain.position.z);

    }
}
