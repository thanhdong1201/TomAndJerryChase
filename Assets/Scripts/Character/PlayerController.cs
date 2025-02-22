﻿using UnityEngine;

public enum PlayerState
{
    Idle,     
    Running,          
    Hit,        
    Magnet,         
    Shielded,       
    Invisible,
    SpeedBoost,
    Dead           
}
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private Animator animator;

    private PlayerMovement playerMovement;
    private PlayerInputHandler playerInputHandler;
    private CharacterController characterController;
    private Rigidbody rb;

    private bool isGameStarted = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        characterController = GetComponent<CharacterController>();
        Idle();
    }
    private void OnEnable()
    {
        gameStateSO.OnGameStateChanged += HandleGameStateChange;
    }
    private void OnDisable()
    {
        gameStateSO.OnGameStateChanged -= HandleGameStateChange;
    }
    private void Update()
    {
        if (!isGameStarted) return;

        playerMovement.CheckGround();
        playerMovement.UpdateVerticalVelocity();
        playerMovement.MoveCharacter();
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
                isGameStarted = false;
                break;
            case GameState.Pause:
                isGameStarted = false;
                break;
            case GameState.Resume:
                isGameStarted = true;
                break;
        }
    }
    private void Idle()
    {
        animator.SetBool("Run", false);
        isGameStarted = false;
    }
    private void StartGame()
    {
        isGameStarted = true;
        animator.SetBool("Run", true);
        gameStateSO.SetPlayerState(PlayerState.Running);
    }
    private void RestartGame()
    {
        if (rb != null)
        {
            Destroy(rb);
            rb = null;
        }

        animator.SetTrigger("Restart");
        playerMovement.ResetPosition();
        Idle();
    }
    public void GetHit()
    {
        animator.SetTrigger("Hit");

        float slowFactor = 0.75f;
        float slowDuration = 2f;
        playerMovement.ChangeSpeed(slowFactor, slowDuration);
    }
    public void Die(Vector3 force)
    {
        gameStateSO.SetGameState(GameState.GameOver);
        gameStateSO.SetPlayerState(PlayerState.Dead);
        isGameStarted = false;
        animator.SetTrigger("Fall");
        characterController.enabled = false;

        AddForce(force);
    }
    private void AddForce(Vector3 force)
    {
        if (rb != null) return;
        rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 1f;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
    }
}
