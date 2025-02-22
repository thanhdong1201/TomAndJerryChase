using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateMachine
{
    protected EnemyController enemy;

    public EnemyStateMachine(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public virtual void Enter() { } 
    public virtual void Update() { } 
    public virtual void Exit() { }   
}
