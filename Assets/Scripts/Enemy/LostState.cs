using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy States/Lost")]
public class LostState : EnemyStateMachine
{
    [SerializeField] private float slowDownDuration = 3f;
    private float elapsedTime;
    private float initialSpeed;

    public override void Enter()
    {
        enemy.gameStateSO.SetEnemyState(EnemyState.Lost);
        enemy.gameStateSO.OnPlayerStateChanged += HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision += HandleObstacleCollision;

        initialSpeed = enemy.currentSpeed;
        elapsedTime = 0f;
    }
    public override void Update() 
    {
        enemy.MoveToTarget();

        if (elapsedTime < slowDownDuration)
        {
            elapsedTime += Time.deltaTime;
            enemy.currentSpeed = Mathf.Lerp(initialSpeed, 0f, elapsedTime / slowDownDuration);
        }
        else
        {
            enemy.SetState(enemy.lostState);
        }
    }
    public override void Exit()
    {
        enemy.gameStateSO.OnPlayerStateChanged -= HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision -= HandleObstacleCollision;
    }
    private void HandleObstacleCollision()
    {
        enemy.SetState(enemy.chaseState);
    }
    private void HandlePlayerStateChanged(PlayerState state)
    {
        if (state == PlayerState.Dead) enemy.SetState(enemy.idleState);
        if (state == PlayerState.Invisible || state == PlayerState.SpeedBoost) enemy.SetState(enemy.lostState);
    }
}
