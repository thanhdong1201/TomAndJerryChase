using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy States/RangeAttack")]
public class RangeAttackState : EnemyStateMachine
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float defaultEscapeTimer = 25f;
    [SerializeField] private float defaultCoolDownAttack = 2f;
    private float coolDownAttack;
    private float escapeTimer;

    public override void Enter()
    {
        enemy.gameStateSO.SetEnemyState(EnemyState.Lost);
        enemy.gameStateSO.OnPlayerStateChanged += HandlePlayerStateChanged;

        coolDownAttack = defaultCoolDownAttack;
        escapeTimer = defaultEscapeTimer;
    }

    public override void Update()
    {
        coolDownAttack -= Time.deltaTime;
        if ((coolDownAttack <= 0))
        {
            coolDownAttack = defaultCoolDownAttack;
            enemy.RangeAttack(projectilePrefab);
        }

        escapeTimer -= Time.deltaTime;
        if (escapeTimer <= 0)
        {
            enemy.SetState(enemy.lostState);
        }
    }
    public override void Exit()
    {
        enemy.gameStateSO.OnPlayerStateChanged -= HandlePlayerStateChanged;
    }
    private void HandlePlayerStateChanged(PlayerState state)
    {
        if (state == PlayerState.Dead) enemy.SetState(enemy.idleState);
    }
}
