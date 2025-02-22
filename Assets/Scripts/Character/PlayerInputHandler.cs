using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private Vector2 startTouchPosition, endTouchPosition;
    private float minSwipeDistance = 50f;
    private bool isSwiping = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) playerMovement.ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) playerMovement.ChangeLane(1);
        if (Input.GetKeyDown(KeyCode.UpArrow)) playerMovement.Jump();
        if (Input.GetKeyDown(KeyCode.DownArrow)) playerMovement.Slide();

        // Mouse input (PC)
        if (Input.GetMouseButtonDown(0))
        {
            isSwiping = true;
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            endTouchPosition = Input.mousePosition;
            DetectSwipe();
            isSwiping = false;
        }

        // Touch input (Mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isSwiping = true;
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                endTouchPosition = touch.position;
                DetectSwipe();
                isSwiping = false;
            }
        }
    }

    private void DetectSwipe()
    {
        float swipeDistanceX = endTouchPosition.x - startTouchPosition.x;
        float swipeDistanceY = endTouchPosition.y - startTouchPosition.y;

        if (Mathf.Abs(swipeDistanceX) > Mathf.Abs(swipeDistanceY)) // Horizontal swipe
        {
            if (Mathf.Abs(swipeDistanceX) > minSwipeDistance)
                playerMovement.ChangeLane(swipeDistanceX > 0 ? 1 : -1);
        }
        else // Vertical swipe
        {
            if (Mathf.Abs(swipeDistanceY) > minSwipeDistance)
            {
                if (swipeDistanceY > 0) playerMovement.Jump(); // Swipe Up
                else playerMovement.Slide(); // Swipe Down
            }
        }
    }
}
