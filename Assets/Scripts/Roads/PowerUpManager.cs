using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs;
    private Queue<GameObject> powerUpPool = new();
    private List<GameObject> activePowerUps = new();

    [Header("Listen to Events")]
    [SerializeField] private GameObjectEventChannelSO powerUpEvent;

    public int GetPoolCount() => powerUpPool.Count;
    public int GetActiveCount() => activePowerUps.Count;

    private void OnEnable()
    {
        powerUpEvent.OnEventRaised += HandleRecyleCollectedPowerUp;
    }
    private void OnDisable()
    {
        powerUpEvent.OnEventRaised -= HandleRecyleCollectedPowerUp;
    }
    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        foreach (GameObject prefab in powerUpPrefabs)
        {
            GameObject powerUp = Instantiate(prefab);
            powerUp.SetActive(false);
            powerUpPool.Enqueue(powerUp);
        }
    }
    public void SpawnPowerUp(RoadSegment segment)
    {
        if (segment?.powerUpPoints == null) return;

        Transform spawnPoint = segment.powerUpPoints[Random.Range(0, segment.powerUpPoints.Length)];

        if (powerUpPool.Count > 0)
        {
            GameObject powerUp = powerUpPool.Dequeue();
            powerUp.transform.position = spawnPoint.position;
            powerUp.SetActive(true);
            activePowerUps.Add(powerUp);
        }
    }

    public void RecyclePassedPowerUps(float playerZ, float safeZone)
    {
        for (int i = activePowerUps.Count - 1; i >= 0; i--)
        {
            GameObject powerUp = activePowerUps[i];
            if (powerUp.transform.position.z < playerZ - safeZone)
            {
                Recycle(powerUp);
            }
        }
    }
    private void HandleRecyleCollectedPowerUp(GameObject powerUp)
    {
        if (!activePowerUps.Contains(powerUp)) return;
        Recycle(powerUp);

    }
    public void ResetPowerUps()
    {
        for (int i = activePowerUps.Count - 1; i >= 0; i--)
        {
            Recycle(activePowerUps[i]);
        }
        activePowerUps.Clear();
    }
    private void Recycle(GameObject powerUp)
    {
        powerUp.SetActive(false);
        powerUp.transform.SetParent(transform);
        powerUpPool.Enqueue(powerUp);
        activePowerUps.Remove(powerUp);
    }
}
