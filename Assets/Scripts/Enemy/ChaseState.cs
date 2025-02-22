using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyStateMachine
{
    public ChaseState(EnemyController enemy) : base(enemy) { }

    private float escapeTimer;
    private float defaultEscapeTimer = 10f;

    public override void Enter()
    {
        enemy.gameStateSO.OnPlayerStateChanged += HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision += HandleObstacleCollision;
        enemy.gameStateSO.SetEnemyState(EnemyState.Chasing);
        enemy.animator.SetBool("Chase", true);
        enemy.StartChase();
        escapeTimer = defaultEscapeTimer;
    }

    public override void Update()
    {
        enemy.MoveToTarget();
        enemy.animator.SetBool("Chase", true);
        escapeTimer -= Time.deltaTime;
        if (escapeTimer <= 0)
        {
            enemy.SetState(new LostState(enemy));
        }
    }

    public override void Exit()
    {
        enemy.gameStateSO.OnPlayerStateChanged -= HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision -= HandleObstacleCollision;
    }
    private void HandleObstacleCollision()
    {
        enemy.SetState(new AttackState(enemy)); // Lần thứ nhất gọi → Tấn công
    }
    private void HandlePlayerStateChanged(PlayerState state)
    {
        if (state == PlayerState.Dead) enemy.SetState(new IdleState(enemy));
        if (state == PlayerState.Invisible || state == PlayerState.SpeedBoost) enemy.SetState(new LostState(enemy));
    }
}
