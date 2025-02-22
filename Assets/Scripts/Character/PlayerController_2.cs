//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem.XR;

////public enum PlayerState
////{
////    Idle,
////    Running,
////    Jumping,
////    Sliding,
////    Hit,
////    Magnet,
////    Shielded,
////    Invisible,
////    SpeedBoost,
////    GainLife,
////    Dead
////}

//public class PlayerController_2 : MonoBehaviour
//{
//    public GameStateSO gameStateSO;

//    [Header("Movement Settings")]
//    public float targetSpeed = 8f;
//    [SerializeField] private float laneDistance = 3f;
//    [SerializeField] private float laneChangeSpeed = 10f;

//    [Header("Ground And Gravity")]
//    [SerializeField] private LayerMask groundLayer;
//    [SerializeField] private float jumpForce = 12f;
//    [SerializeField] private float gravity = -30f;
//    private float fallMultiplier = 2.5f;  // Tăng tốc độ rơi xuống
//    private float lowJumpMultiplier = 2f; // Giúp nhảy lên tự nhiên hơn

//    [Header("Others")]
//    [SerializeField] private Animator animator;
//    private CharacterController characterController;
//    private Rigidbody rb;

//    [Header("Effect")]
//    [SerializeField] private ParticleSystem stunEffect;

//    private float verticalVelocity = 0f;
//    private int currentLane = 0;
//    private Vector3 targetPosition;
//    private Vector3 moveDirection;
//    private bool isJumping = false;
//    private bool isSliding = false;
//    private bool isGrounded = true;
//    private bool isGameStarted = false;

//    //mobile input
//    private Vector2 startTouchPosition, endTouchPosition;
//    private float minSwipeDistance = 50f;
//    private bool isSwiping = false;

//    private void Start()
//    {
//        Idle();
//        characterController = GetComponent<CharacterController>();
//        targetPosition = transform.position;
//        gameStateSO.OnGameStateChanged += HandleGameStateChange;
//        isGameStarted = false;

//    }
//    private void OnDisable()
//    {
//        gameStateSO.OnGameStateChanged -= HandleGameStateChange;
//    }
//    private void HandleGameStateChange(GameState newState)
//    {
//        switch (newState)
//        {
//            case GameState.Start:
//                StartGame();
//                break;
//            case GameState.Restart:
//                RestartGame();
//                break;
//            case GameState.GameOver:
//                //Die();
//                break;
//            case GameState.Pause:
//                isGameStarted = false;
//                break;
//            case GameState.Resume:
//                isGameStarted = true;
//                break;
//        }
//    }
//    private void Update()
//    {
//        if (!isGameStarted) return;

//        HandleInput();
//        CheckGround();
//        UpdateVerticalVelocity();
//        MoveCharacter();
//    }
//    private void HandleInput()
//    {
//        if (Input.GetKeyDown(KeyCode.LeftArrow))
//            ChangeLane(-1);
//        if (Input.GetKeyDown(KeyCode.RightArrow))
//            ChangeLane(1);
//        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isSliding)
//            Jump();
//        if (Input.GetKeyDown(KeyCode.DownArrow))
//        {
//            if (isGrounded && !isSliding) Slide();
//            else if (!isGrounded && !isSliding) Roll();
//        }

//        // Hỗ trợ chuột (PC)
//        if (Input.GetMouseButtonDown(0))
//        {
//            isSwiping = true;
//            startTouchPosition = Input.mousePosition;
//        }
//        else if (Input.GetMouseButtonUp(0) && isSwiping)
//        {
//            endTouchPosition = Input.mousePosition;
//            DetectSwipe();
//            isSwiping = false;
//        }

//        // Hỗ trợ cảm ứng (Mobile)
//        if (Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);
//            if (touch.phase == TouchPhase.Began)
//            {
//                isSwiping = true;
//                startTouchPosition = touch.position;
//            }
//            else if (touch.phase == TouchPhase.Moved)  // Cập nhật vị trí khi vuốt
//            {
//                endTouchPosition = touch.position;
//            }
//            else if (touch.phase == TouchPhase.Ended && isSwiping)
//            {
//                endTouchPosition = touch.position;
//                DetectSwipe();
//                isSwiping = false;
//            }
//        }
//    }

//    private void DetectSwipe()
//    {
//        float swipeDistanceX = endTouchPosition.x - startTouchPosition.x;
//        float swipeDistanceY = endTouchPosition.y - startTouchPosition.y;

//        if (Mathf.Abs(swipeDistanceX) > Mathf.Abs(swipeDistanceY)) // Vuốt ngang
//        {
//            if (Mathf.Abs(swipeDistanceX) > minSwipeDistance)
//            {
//                ChangeLane(swipeDistanceX > 0 ? 1 : -1);
//            }
//        }
//        else // Vuốt dọc
//        {
//            if (Mathf.Abs(swipeDistanceY) > minSwipeDistance)
//            {
//                if (swipeDistanceY > 0 && isGrounded)  // Vuốt lên
//                {
//                    Jump();
//                }
//                else if (swipeDistanceY < 0)  // Vuốt xuống
//                {
//                    if (isGrounded && !isSliding)
//                        Slide();
//                    else if (!isGrounded && !isSliding)
//                        Roll();
//                }
//            }
//        }
//    }

//    private void ChangeLane(int direction)
//    {
//        int targetLane = currentLane + direction;
//        if (targetLane < -1 || targetLane > 1)
//            return;

//        currentLane = targetLane;
//        targetPosition = new Vector3(targetLane * laneDistance, transform.position.y, transform.position.z);

//    }
//    private void CheckGround()
//    {
//        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, 1f, groundLayer);
//        animator.SetBool("Grounded", isGrounded);
//    }
//    private void UpdateVerticalVelocity()
//    {
//        if (!isGrounded)
//        {
//            if (verticalVelocity > 0) //Go Up
//            {
//                verticalVelocity += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
//            }
//            else // Go Down
//            {
//                verticalVelocity += gravity * (fallMultiplier - 1) * Time.deltaTime;
//            }
//        }
//        else if (verticalVelocity < 0)
//        {
//            verticalVelocity = -2f;
//        }
//    }
//    private void MoveCharacter()
//    {
//        float newX = Mathf.MoveTowards(transform.position.x, targetPosition.x, laneChangeSpeed * Time.deltaTime);
//        float forwardDisplacement = targetSpeed * Time.deltaTime;
//        float verticalDisplacement = verticalVelocity * Time.deltaTime;

//        Vector3 movement = new Vector3(newX - transform.position.x, verticalDisplacement, forwardDisplacement);
//        characterController.Move(movement);
//    }
//    private void Jump()
//    {
//        if (!isGrounded || isJumping) return;

//        gameStateSO.SetPlayerState(PlayerState.Jumping);
//        isJumping = true;
//        animator.SetTrigger("Jump");

//        verticalVelocity = jumpForce;

//        StartCoroutine(ResetJump());
//    }
//    private IEnumerator ResetJump()
//    {
//        while (!isGrounded)
//        {
//            yield return null;
//        }
//        isJumping = false;
//        gameStateSO.SetPlayerState(PlayerState.Running);
//    }

//    private void Slide()
//    {
//        if (isSliding || isJumping) return;

//        gameStateSO.SetPlayerState(PlayerState.Sliding);
//        isSliding = true;
//        animator.SetTrigger("Slide");
//        StartCoroutine(ResetSlide());
//    }
//    private IEnumerator ResetSlide()
//    {
//        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
//        {
//            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
//            {
//                break;
//            }
//            yield return null;
//        }

//        isSliding = false;
//        gameStateSO.SetPlayerState(PlayerState.Running);
//    }
//    private void Roll()
//    {
//        gameStateSO.SetPlayerState(PlayerState.Sliding);
//        animator.SetTrigger("Roll");
//        verticalVelocity = -jumpForce * 1.5f;
//        StartCoroutine(ResetRoll());
//    }
//    private IEnumerator ResetRoll()
//    {
//        while (!isGrounded)
//        {
//            yield return null;
//        }
//        gameStateSO.SetPlayerState(PlayerState.Running);
//    }
//    public void GetHit()
//    {
//        gameStateSO.SetPlayerState(PlayerState.Hit);
//        animator.SetTrigger("Hit");
//        stunEffect.gameObject.SetActive(true);
//        stunEffect.Play();

//        float slowFactor = 0.75f;
//        float slowDuration = 2f;
//        StartCoroutine(ChangeSpeedDuration(slowFactor, slowDuration));
//    }
//    public IEnumerator ChangeSpeedDuration(float factor, float duration)
//    {
//        float originalSpeed = targetSpeed;
//        targetSpeed *= factor;
//        yield return new WaitForSeconds(duration);
//        targetSpeed = originalSpeed;
//        gameStateSO.SetPlayerState(PlayerState.Running);
//    }
//    public void Die(Vector3 force)
//    {
//        gameStateSO.SetGameState(GameState.GameOver);
//        gameStateSO.SetPlayerState(PlayerState.Dead);
//        isGameStarted = false;
//        animator.SetTrigger("Fall");
//        characterController.enabled = false;

//        if (rb != null) return;
//        rb = gameObject.AddComponent<Rigidbody>();
//        rb.mass = 0.5f;
//        rb.isKinematic = false;
//        rb.useGravity = true;
//        rb.freezeRotation = true;
//        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
//        rb.velocity = Vector3.zero;
//        rb.AddForce(force, ForceMode.Impulse);
//    }
//    private void Idle()
//    {
//        animator.SetBool("Run", false);
//        isGameStarted = false;
//    }
//    private void StartGame()
//    {
//        isGameStarted = true;
//        animator.SetBool("Run", true);
//        gameStateSO.SetPlayerState(PlayerState.Running);
//    }
//    private void RestartGame()
//    {
//        if (rb != null)
//        {
//            Destroy(rb);
//        }

//        animator.SetTrigger("Restart");
//        Idle();
//        stunEffect.gameObject.SetActive(false);
//        characterController.enabled = false;
//        currentLane = 0;
//        transform.position = new Vector3(0, 0, -15);
//        targetPosition = transform.position;
//        characterController.enabled = true;

//    }
//}
