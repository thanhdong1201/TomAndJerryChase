using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleData
{
    public GameObject prefab;
    public float spawnChance;
}

public class ObstacleManager : MonoBehaviour
{
    private Dictionary<GameObject, Queue<GameObject>> obstaclePools = new();
    private List<GameObject> activeObstacles = new();
    private Dictionary<GameObject, GameObject> obstacleInstanceToPrefab = new();

    public void SpawnObstacles(RoadSegment segment)
    {
        if (segment?.obstaclePoints == null || segment.allowedObstacles.Length == 0) return;

        foreach (Transform point in segment.obstaclePoints)
        {
            float value = Random.value;

            if (value <= 0.5f) //50% chance spawn obstacle at this point
            {
                var selectedObstacle = GetRandomObstacleData(segment.allowedObstacles); // get random obstacle from obstacles
                if (selectedObstacle != null) SpawnObstacle(point.position, selectedObstacle);
            }
        }
    }
    private void SpawnObstacle(Vector3 position, ObstacleData obstacleData)
    {
        if (!obstaclePools.TryGetValue(obstacleData.prefab, out var pool))
        {
            pool = new Queue<GameObject>();
            obstaclePools[obstacleData.prefab] = pool;
        }

        GameObject obstacle = pool.Count > 0 ? pool.Dequeue() : Instantiate(obstacleData.prefab);
        obstacle.transform.position = position;
        obstacle.SetActive(true);

        activeObstacles.Add(obstacle);
        obstacleInstanceToPrefab[obstacle] = obstacleData.prefab;
    }

    public void RecyclePassedObstacles(float playerZ, float safeZone)
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            var obstacle = activeObstacles[i];
            if (obstacle.transform.position.z < playerZ - safeZone)
            {
                Recycle(obstacle);
            }
        }
    }

    private void Recycle(GameObject obstacle)
    {
        if (obstacleInstanceToPrefab.TryGetValue(obstacle, out GameObject prefab))
        {
            obstacle.SetActive(false);
            obstaclePools[prefab].Enqueue(obstacle);
            activeObstacles.Remove(obstacle);
            obstacleInstanceToPrefab.Remove(obstacle);
        }
    }

    public void ResetObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            Recycle(activeObstacles[i]);
        }
        activeObstacles.Clear();
    }

    private ObstacleData GetRandomObstacleData(ObstacleData[] allowedObstacles)
    {
        float totalChance = 0f;
        foreach (var data in allowedObstacles)
        {
            totalChance += data.spawnChance;
        }

        float randomPoint = Random.value * totalChance;
        float cumulative = 0f;

        foreach (var data in allowedObstacles)
        {
            cumulative += data.spawnChance;
            if (randomPoint <= cumulative)
                return data;
        }

        return null;
    }
}
