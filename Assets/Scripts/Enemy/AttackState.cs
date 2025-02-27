using UnityEngine;

[CreateAssetMenu(menuName = "Enemy States/Attack")]
public class AttackState : EnemyStateMachine
{
    [SerializeField] private AudioCueSO audioCueSO;
    [SerializeField] private float defaultEscapeTimer = 10f;
    [SerializeField] private float defaultCoolDownAttack = 2.5f;
    [SerializeField] private float attackDistance = 3.5f;

    [Header("Broadcasting from Events")]
    [SerializeField] private BoolEventChannelSO enableChaseCameraEvent;

    private float coolDownAttack;
    private float escapeTimer;

    public override void Enter()
    {
        enemy.gameStateSO.SetEnemyState(EnemyState.Attacking);
        enemy.gameStateSO.OnPlayerStateChanged += HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision += HandleObstacleCollision;

        escapeTimer = defaultEscapeTimer;
        coolDownAttack = 0f;
        enemy.followDistance = attackDistance;
    }

    public override void Update()
    {
        enemy.MoveToTarget(enemy.playerMovement.getSpeed);


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
            enemy.SetState(enemy.lostState);
        }
    }
    public override void Exit()
    {
        enemy.gameStateSO.OnPlayerStateChanged -= HandlePlayerStateChanged;
        enemy.gameStateSO.OnObstacleCollision -= HandleObstacleCollision;
        audioCueSO.FadeOutMusic();
        enableChaseCameraEvent.RaiseEvent(false);
    }
    private void HandleObstacleCollision()
    {
        escapeTimer = defaultEscapeTimer; // Reset timer mỗi khi va chạm
    }
    private void HandlePlayerStateChanged(PlayerState state)
    {
        if (state == PlayerState.Dead) enemy.SetState(enemy.idleState);
        if (state == PlayerState.Invisible || state == PlayerState.SpeedBoost) enemy.SetState(enemy.lostState);
    }
}
