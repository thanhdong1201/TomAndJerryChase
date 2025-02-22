using System.Collections;
using UnityEngine;

public class LostState : EnemyStateMachine
{
    public LostState(EnemyController enemy) : base(enemy) { }

    private float slowDownDuration = 3f;
    private float elapsedTime;
    private float initialSpeed;

    public override void Enter()
    {
        enemy.gameStateSO.SetEnemyState(EnemyState.Lost);
        initialSpeed = enemy.currentSpeed;
        elapsedTime = 0f;

        enemy.followDistance = enemy.defaultFollowDistance;

        enemy.gameStateSO.OnPlayerStateChanged += HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision += HandleObstacleCollision;
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
            enemy.StopChase();
        }
    }
    public override void Exit()
    {
        enemy.gameStateSO.OnPlayerStateChanged -= HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision -= HandleObstacleCollision;
    }
    private void HandleObstacleCollision()
    {
        enemy.SetState(new ChaseState(enemy));
    }
    private void HandlePlayerStateChanged(PlayerState state)
    {
        if (state == PlayerState.Dead) enemy.SetState(new IdleState(enemy));
        if (state == PlayerState.Invisible || state == PlayerState.SpeedBoost) enemy.SetState(new LostState(enemy));
    }
}
