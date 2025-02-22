using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float targetSpeed = 8f;
    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float laneChangeSpeed = 10f;

    [Header("Ground & Gravity")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravity = -30f;

    [Header("Others")]
    [SerializeField] private Animator animator;
    private CharacterController characterController;

    private Vector3 targetPosition;
    private int currentLane = 0;
    private float verticalVelocity = 0f;
    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;
    private bool isJumping = false;
    private bool isSliding = false;
    private bool isGrounded = true;
    public float getSpeed => targetSpeed;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        targetPosition = transform.position;
    }

    public void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, 1f, groundLayer);
        animator.SetBool("Grounded", isGrounded);
    }

    public void UpdateVerticalVelocity()
    {
        if (!isGrounded)
        {
            if (verticalVelocity > 0) //Go Up
            {
                verticalVelocity += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            else // Go Down
            {
                verticalVelocity += gravity * (fallMultiplier - 1) * Time.deltaTime;
            }
        }
        else if (verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
    }

    public void MoveCharacter()
    {
        float newX = Mathf.MoveTowards(transform.position.x, targetPosition.x, laneChangeSpeed * Time.deltaTime);
        float forwardDisplacement = targetSpeed * Time.deltaTime;
        float verticalDisplacement = verticalVelocity * Time.deltaTime;

        Vector3 movement = new Vector3(newX - transform.position.x, verticalDisplacement, forwardDisplacement);
        characterController.Move(movement);
    }

    public void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < -1 || targetLane > 1) return;

        currentLane = targetLane;
        targetPosition = new Vector3(targetLane * laneDistance, transform.position.y, transform.position.z);
    }

    public void Jump()
    {
        if (!isGrounded || isJumping) return;

        isJumping = true;
        verticalVelocity = jumpForce;

        animator?.SetTrigger("Jump");

        StartCoroutine(ResetJump());
    }

    private IEnumerator ResetJump()
    {
        while (!isGrounded) yield return null;
        isJumping = false;
    }

    public void Slide()
    {
        if (!isGrounded)
        {
            verticalVelocity = -jumpForce * 1.5f;
            animator.SetTrigger("Roll");
            //StartCoroutine(ResetRoll());
        }

        if (isSliding) return;


        if (isGrounded )
        {
            isSliding = true;
            animator.SetTrigger("Slide");
            StartCoroutine(ResetSlide());
        }

    }
    private IEnumerator ResetSlide()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
            yield return null;
        }
        isSliding = false;
    }
    private IEnumerator ResetRoll()
    {
        while (!isGrounded)
        {
            yield return null;
        }
        isSliding = false;
    }
    public void ChangeSpeed(float factor, float duration)
    {
        StartCoroutine(ChangeSpeedDuration(factor, duration));
    }
    private IEnumerator ChangeSpeedDuration(float factor, float duration)
    {
        float originalSpeed = targetSpeed;
        targetSpeed *= factor;
        yield return new WaitForSeconds(duration);
        targetSpeed = originalSpeed;
    }
    public void ResetPosition()
    {
        characterController.enabled = false;
        currentLane = 0;
        transform.position = new Vector3(0, 0, -15);
        targetPosition = transform.position;
        characterController.enabled = true;
    }
}
