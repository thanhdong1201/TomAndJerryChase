using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerData", fileName = "PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public event Action OnDataChanged;
    private int _currentCoin;
    public int currentCoin
    {
        get => _currentCoin;
        set
        {
            _currentCoin = value;
            OnDataChanged?.Invoke();
        }
    }

    private int _currentScore;
    public int currentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            OnDataChanged?.Invoke();
        }
    }

    private float _currentDistance;
    public float currentDistance
    {
        get => _currentDistance;
        set
        {
            _currentDistance = value;
            OnDataChanged?.Invoke();
        }
    }
    private void OnEnable()
    {
        Restart();
    }
    public void Restart()
    {
        currentCoin = 0;
        currentScore = 0;
        currentDistance = 0;

        currentCoin += 0;
    }
    public void AddCurrentScore(int score)
    {
        currentScore += score;
    }
    public void AddCurrentDistance(int distance)
    {
        currentDistance += distance;
    }
    public void AddCurrentCoin(int coin)
    {
        currentCoin += coin;
    }
}
