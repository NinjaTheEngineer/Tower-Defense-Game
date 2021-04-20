 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }

    public event EventHandler OnWaveNumberChanged;
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
        Offline
    }

    private State state;

    [SerializeField] private Transform nextWaveSpawnPositonTransform;
    private List<Transform> spawnPositionTransformList;
    public GameObject enemySpawnPoint;

    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int waveNumber;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    private void Awake()
    {
        Instance = this;
        state = State.Offline;
    }
    public void StartEnemyWaveManager()
    {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositonTransform.position = spawnPosition;
        nextWaveSpawnTimer = 3f;
    }

    public void SetSpawnPositionList(Vector3 centerPosition, float maxWidth, float maxLength)
    {
        spawnPositionTransformList = new List<Transform>();
        List<Vector3> listOfPositions = GetListOfPossibleEnemyPositions(centerPosition, maxWidth, maxLength);
        foreach(Vector3 position in listOfPositions)
        {
            GameObject point = Instantiate(enemySpawnPoint, position, enemySpawnPoint.transform.rotation);
            spawnPositionTransformList.Add(point.transform);
        }

        StartEnemyWaveManager();
    }

    private void Update()
    {
        if(state != State.Offline)
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, .3f);
                        //spawnPosition = UtilsClass.GetMouseWorldPosition() + UtilsClass.GetRandomDir() * 4f;
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;

                        if(remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositonTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;
        }
        

    }

    private void SpawnWave()
    {
        remainingEnemySpawnAmount = 3 + 2 * waveNumber;
        state = State.SpawningWave;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }

    private List<Vector3> GetListOfPossibleEnemyPositions(Vector3 position, float width, float length)
    {
        float x = width / 4;
        float y = length / 4;
        return new List<Vector3>
    {
        new Vector3(position.x - x, position.y + (y * 2), 0),
        new Vector3(position.x + x, position.y + (y * 2), 0),
        new Vector3(position.x - x, position.y - (y * 2), 0),
        new Vector3(position.x + x, position.y - (y * 2), 0),
        new Vector3(position.x - (x * 2), position.y - y, 0),
        new Vector3(position.x - (x * 2), position.y + y, 0),
        new Vector3(position.x + (x * 2), position.y - y, 0),
        new Vector3(position.x + (x * 2), position.y + y, 0)
    };
    }
}
