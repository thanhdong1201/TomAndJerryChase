using Cinemachine;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private GameObject startCamera;
    [SerializeField] private GameObject chaseCamera;

    [Header("Listen to Events")]
    [SerializeField] private BoolEventChannelSO enableChaseCameraEvent;

    private Coroutine restartCoroutine;

    private void OnEnable()
    {
        gameStateSO.OnGameStateChanged += HandleGameStateChange;
        enableChaseCameraEvent.OnEventRaised += HandleChaseCamera;
    }
    private void OnDisable()
    {
        gameStateSO.OnGameStateChanged -= HandleGameStateChange;
        enableChaseCameraEvent.OnEventRaised -= HandleChaseCamera;
    }
    private void HandleGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Start:
                StartGame();
                break;
            case GameState.Restart:
                RestartGame();
                break;
        }
    }
    private void HandleChaseCamera(bool value)
    {
        if (value)
        {
            cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
            chaseCamera.SetActive(true);
        }
        else
        {
            chaseCamera.SetActive(false);
        }
    }
    private void StartGame()
    {
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
        startCamera.SetActive(false);
    }
    private void RestartGame()
    {
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        chaseCamera.SetActive(false);
        startCamera.SetActive(true);
    }
}