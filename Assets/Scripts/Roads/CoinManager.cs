using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinsPerPoint = 5;
    private Queue<GameObject> coinPool = new();
    private List<GameObject> activeCoins = new();

    [Header("Listen to Events")]
    [SerializeField] private GameObjectEventChannelSO coinEvent;

    public int GetPoolCount() => coinPool.Count;
    public int GetActiveCount() => activeCoins.Count;

    private void OnEnable()
    {
        coinEvent.OnEventRaised += HandleRecycleCollectedCoin;
    }
    private void OnDisable()
    {
        coinEvent.OnEventRaised -= HandleRecycleCollectedCoin;
    }
    public void SpawnCoins(RoadSegment segment)
    {
        if (segment?.coinPoints == null) return;

        foreach (Transform point in segment.coinPoints)
        {
            float value = Random.value;

            if (value < 0.35f) SpawnCoinLine(point.position, Vector3.forward);
            else if (value > 0.65) SpawnCoinArc(point.position);
        }
    }

    private void SpawnCoinLine(Vector3 startPosition, Vector3 direction)
    {
        float coinSpacing = 2f;
        for (int i = 0; i < coinsPerPoint; i++)
        {
            SpawnCoin(startPosition + direction * (i * coinSpacing));
        }
    }
    private void SpawnCoinArc(Vector3 startPosition)
    {
        float coinSpacing = 1.8f, arcHeight = 2f;
        for (int i = 0; i < coinsPerPoint; i++)
        {
            float heightOffset = Mathf.Sin(i * Mathf.PI / (coinsPerPoint - 1)) * arcHeight;
            Vector3 coinPosition = startPosition + Vector3.forward * (i * coinSpacing) + Vector3.up * heightOffset;
            SpawnCoin(coinPosition);
        }
    }
    private void SpawnCoin(Vector3 position)
    {
        GameObject coin = coinPool.Count > 0 ? coinPool.Dequeue() : Instantiate(coinPrefab);
        coin.transform.position = position;
        coin.SetActive(true);
        activeCoins.Add(coin);
    }

    public void RecyclePassedCoins(float playerZ, float safeZone)
    {
        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            GameObject coin = activeCoins[i];
            if (coin.transform.position.z < playerZ - safeZone)
            {
                Recycle(coin);
            }
        }
    }
    private void HandleRecycleCollectedCoin(GameObject coin)
    {
        if (activeCoins.Contains(coin))
        {
            Recycle(coin);
           
        }
    }

    public void ResetCoins()
    {
        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            Recycle(activeCoins[i]);
        }
        activeCoins.Clear();
    }
    private void Recycle(GameObject coin)
    {
        coin.SetActive(false);
        coinPool.Enqueue(coin);
        activeCoins.Remove(coin);
    }
}
