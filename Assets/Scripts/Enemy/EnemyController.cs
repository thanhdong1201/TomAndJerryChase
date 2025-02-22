using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameStateSO gameStateSO;

    [Header("Movement Settings")]
    public float speedMultiplier = 1.75f;
    public float maxSpeed = 20f;
    public float followDistance = 6f;
    public float attackDistance = 3.5f;
    public float spawnDistance = 7f;

    [Header("Components")]
    public Animator animator;
    public GameObject character;

    private EnemyStateMachine currentState;
    private PlayerMovement playerMovement;
    private Transform player;
    private GameObject projectilePrefab;

    [HideInInspector] public float currentSpeed;
    public float defaultFollowDistance { get; private set; }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player?.GetComponent<PlayerMovement>();
        defaultFollowDistance = followDistance;

        SetState(new IdleState(this));
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
        currentState?.Enter();
    }
    public void MoveToTarget()
    {
        float speed = Mathf.Min(playerMovement.getSpeed * speedMultiplier, maxSpeed);
        Vector3 targetPosition = new Vector3(player.position.x, 0f, player.position.z - followDistance);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    public void SetUpProjectile(GameObject projectilePrefab)
    {
        this.projectilePrefab = projectilePrefab;
    }
    public void MissleAttack()
    {
        GameObject missle = Instantiate(projectilePrefab, transform);
        ProjectileController projectile = missle.GetComponent<ProjectileController>();
        Vector3 playerVelocity = player.GetComponent<CharacterController>().velocity;
        projectile.LauchToTarget(player, playerVelocity);
    }
    public void StartChase()
    {
        currentSpeed = playerMovement.getSpeed;
        followDistance = defaultFollowDistance;
        transform.position = new Vector3(player.position.x, 0f, player.position.z - spawnDistance);


        if (!character.activeSelf)
        {
            character.SetActive(true);
        }
    }
    public void StopChase()
    {
        currentSpeed = 0f;
        character.SetActive(false);
    }
    private void StartGame()
    {
        SetState(new ChaseState(this));
        currentSpeed = playerMovement.getSpeed;
        followDistance = defaultFollowDistance;
    }
    private void RestartGame()
    {
        SetState(new IdleState(this));
        character.SetActive(false); 
        transform.position = new Vector3(0, 0, -30);
        character.SetActive(true);
    }
}
