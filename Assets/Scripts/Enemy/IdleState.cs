using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyStateMachine
{
    public IdleState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.gameStateSO.SetEnemyState(EnemyState.Idle);
        enemy.currentSpeed = 0f;
        enemy.animator.SetBool("Walk", false);
        enemy.animator.SetBool("Chase", false);
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        
    }
}
