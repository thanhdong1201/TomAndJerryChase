using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleAttackState : EnemyStateMachine
{
    public MissleAttackState(EnemyController enemy) : base(enemy) { }

    private float escapeTimer = 25f;
    private float coolDownAttack;
    private float defaultCoolDownAttack = 2f;

    public override void Enter()
    {
        enemy.gameStateSO.SetEnemyState(EnemyState.Lost);
        enemy.gameStateSO.OnPlayerStateChanged += HandlePlayerStateChanged;
        coolDownAttack = defaultCoolDownAttack;
    }

    public override void Update()
    {
        coolDownAttack -= Time.deltaTime;
        if ((coolDownAttack <= 0))
        {
            coolDownAttack = defaultCoolDownAttack;
            enemy.MissleAttack();
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
    }
    private void HandlePlayerStateChanged(PlayerState state)
    {
        if (state == PlayerState.Dead) enemy.SetState(new IdleState(enemy));
    }
}
