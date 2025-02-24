using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameStateSO gameStateSO;
    public Animator animator;

    [Header("State Machine")]
    public IdleState idleState;
    public ChaseState chaseState;
    public AttackState attackState;
    public LostState lostState;
    public PlayerMovement playerMovement { get; private set; }
    public Transform player { get; private set; }

    [HideInInspector] public float followDistance;
    [HideInInspector] public float currentSpeed;

    private EnemyStateMachine currentState;
    private GameObject projectilePrefab;

    private float speedMultiplier = 1.75f;
    private float maxSpeed = 20f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player?.GetComponent<PlayerMovement>();
        SetState(idleState);
    }
    private void OnEnable()
    {
        gameStateSO.OnGameStateChanged += HandleGameStateChange;
    }
    private void OnDisable()
    {
        gameStateSO.OnGameStateChanged -= HandleGameStateChange;
    }
    private void HandleGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Start:
                StartGame();
                break;
            case GameState.Restart:
                RestartGame();
                break;
            case GameState.GameOver:
                break;
        }
    }
    private void Update()
    {
        currentState?.Update();
    }
    public void SetState(EnemyStateMachine newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Initialize(this);
        currentState?.Enter();
    }
    public void MoveToTarget()
    {
        float speed = Mathf.Min(playerMovement.getSpeed * speedMultiplier, maxSpeed);
        Vector3 targetPosition = new Vector3(player.position.x, 0f, player.position.z - followDistance);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    public void RangeAttack(GameObject projectilePrefab)
    {
        GameObject missle = Instantiate(projectilePrefab, transform);
        ProjectileController projectile = missle.GetComponent<ProjectileController>();
        Vector3 playerVelocity = player.GetComponent<CharacterController>().velocity;
        projectile.LauchToTarget(player, playerVelocity);
    }
    private void StartGame()
    {
        SetState(chaseState);
        currentSpeed = playerMovement.getSpeed;
    }
    private void RestartGame()
    {
        SetState(idleState);
        transform.position = new Vector3(0, 0, -30);
    }
}
