using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private GameObject[] roadPrefabs;
    [SerializeField] private int maxActiveRoads = 3; // Giới hạn số lượng road active
    [SerializeField] private float safeZone = 120f;
    [SerializeField] private float spawnZ = 0f;
    private float roadLength = 100f;

    private Transform player;
    private Queue<GameObject> roadPool = new();
    private List<GameObject> activeRoads = new();
    private bool isGameStarted = false;
    private float defaultSpawnZ;

    [SerializeField] private CoinManager coinManager;
    [SerializeField] private PowerUpManager powerUpManager;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private bool allowDebug = false;
    [SerializeField] private TextMeshProUGUI debugText;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        defaultSpawnZ = spawnZ;
        InitializePool();
        InitializeRoads();
    }
    private void OnEnable()
    {
        gameStateSO.OnGameStateChanged += HandleGameStateChange;
    }
    private void OnDisable()
    {
        gameStateSO.OnGameStateChanged -= HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState newState)
    {
        isGameStarted = newState == GameState.Start;
        if (newState == GameState.Restart) Restart();
    }

    private void Update()
    {
        if (!isGameStarted || player == null) return;
        if (player.position.z - safeZone > spawnZ - (maxActiveRoads * roadLength))
        {
            SpawnRoad();
        }
        coinManager.RecyclePassedCoins(player.position.z, 5f);
        powerUpManager.RecyclePassedPowerUps(player.position.z, 5f);
        obstacleManager.RecyclePassedObstacles(player.position.z, 5f);
        DebugObjectPools();
    }

    private void InitializePool()
    {
        foreach (var prefab in roadPrefabs)
        {
            GameObject road = Instantiate(prefab);
            road.SetActive(false);
            roadPool.Enqueue(road);
        }
    }

    private void InitializeRoads()
    {
        for (int i = 0; i < maxActiveRoads; i++)
        {
            SpawnRoad();
        }
    }

    private void SpawnRoad()
    {
        if (roadPool.Count == 0) return;

        GameObject road = roadPool.Dequeue(); 
        road.transform.position = Vector3.forward * spawnZ;
        road.SetActive(true);
        activeRoads.Add(road);
        spawnZ += roadLength;

        if (activeRoads.Count > maxActiveRoads)
        {
            RecycleRoad(activeRoads[0]);
        }

        if (road.TryGetComponent(out RoadSegment segment))
        {
            coinManager.SpawnCoins(segment);
            powerUpManager.SpawnPowerUp(segment);
            obstacleManager.SpawnObstacles(segment);
        }
    }

    private void RecycleRoad(GameObject road)
    {
        if (!activeRoads.Contains(road)) return;
        road.SetActive(false);
        roadPool.Enqueue(road);
        activeRoads.Remove(road);
    }

    private void Restart()
    {
        spawnZ = defaultSpawnZ;
        foreach (var road in activeRoads)
        {
            road.SetActive(false);
            roadPool.Enqueue(road);
        }
        activeRoads.Clear();

        coinManager.ResetCoins();
        powerUpManager.ResetPowerUps();
        obstacleManager.ResetObstacles();

        InitializeRoads();
        isGameStarted = true;
    }

    private void DebugObjectPools()
    {
        if (debugText == null || !allowDebug) return;

        string debugInfo = "Object Pool Debug\n";
        debugInfo += $"Road Pool: {roadPool.Count} | Active Roads: {activeRoads.Count}\n";
        debugInfo += $"Coins Pool: {coinManager.GetPoolCount()} | Active: {coinManager.GetActiveCount()}\n";
        debugText.text = debugInfo;
    }
}
