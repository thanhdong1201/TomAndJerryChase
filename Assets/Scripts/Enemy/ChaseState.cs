using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy States/Chase")]
public class ChaseState : EnemyStateMachine
{
    [SerializeField] private AudioCueSO audioCueSO;
    [SerializeField] private AudioClip chaseMusic;
    [SerializeField] private float defaultEscapeTimer = 10f;
    [SerializeField] private float followDistance = 6;
    [SerializeField] private float spawnDistance = 8;

    [Header("Broadcasting from Events")]
    [SerializeField] private BoolEventChannelSO enableChaseCameraEvent;

    private float escapeTimer;

    public override void Enter()
    {
        enemy.gameStateSO.SetEnemyState(EnemyState.Chasing);
        enemy.gameStateSO.OnPlayerStateChanged += HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision += HandleObstacleCollision;

        enemy.animator.SetBool("Chase", true);
        enemy.followDistance = followDistance;
        enemy.transform.position = new Vector3(enemy.player.position.x, 0f, enemy.player.position.z - spawnDistance);

        escapeTimer = defaultEscapeTimer;
        audioCueSO.PlayMusic(chaseMusic);
        enableChaseCameraEvent.RaiseEvent(true);
    }

    public override void Update()
    {
        enemy.MoveToTarget(enemy.playerMovement.getSpeed);
        enemy.animator.SetBool("Chase", true);
        escapeTimer -= Time.deltaTime;
        if (escapeTimer <= 0)
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
        enemy.SetState(enemy.attackState); 
    }
    private void HandlePlayerStateChanged(PlayerState state)
    {
        if (state == PlayerState.Dead) enemy.SetState(enemy.idleState);
        if (state == PlayerState.Invisible || state == PlayerState.SpeedBoost) enemy.SetState(enemy.lostState);
    }
}
