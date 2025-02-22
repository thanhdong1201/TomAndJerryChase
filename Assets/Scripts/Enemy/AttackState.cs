using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyStateMachine
{
    public AttackState(EnemyController enemy) : base(enemy) { }

    private float escapeTimer;
    private float defaultEscapeTimer = 10f;
    private float transitionSpeed = 5f;
    private float coolDownAttack;
    private float defaultCoolDownAttack = 2.5f; 
    public override void Enter()
    {
        enemy.gameStateSO.SetEnemyState(EnemyState.Attacking);
        enemy.gameStateSO.OnPlayerStateChanged += HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision += HandleObstacleCollision;
        escapeTimer = defaultEscapeTimer;
        coolDownAttack = 0f;
    }

    public override void Update()
    {
        if (enemy.followDistance > enemy.attackDistance)
        {
            enemy.followDistance = Mathf.MoveTowards(enemy.followDistance, enemy.attackDistance, transitionSpeed * Time.deltaTime);
        }

        enemy.MoveToTarget();


        coolDownAttack -= Time.deltaTime;
        if ((coolDownAttack <= 0))
        {
            coolDownAttack = defaultCoolDownAttack;
            int randomAttack = Random.Range(0, 2);
            enemy.animator.SetInteger("AttackIndex", randomAttack);
            enemy.animator.SetTrigger("Attack");
        }



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
        escapeTimer = defaultEscapeTimer; // Reset timer mỗi khi va chạm
    }
    private void HandlePlayerStateChanged(PlayerState state)
    {
        if (state == PlayerState.Dead) enemy.SetState(new IdleState(enemy));
        if (state == PlayerState.Invisible || state == PlayerState.SpeedBoost) enemy.SetState(new LostState(enemy));
    }
}
