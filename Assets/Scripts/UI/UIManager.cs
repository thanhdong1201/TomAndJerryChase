using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private Canvas canvas;

    [Header("CountdownUI")] 
    [SerializeField] private TextMeshProUGUI countdownText;
    private float countdownTime = 3f;

    [Header("InGameUI")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI warningText;

    [Header("DebugUI")]
    [SerializeField] private TextMeshProUGUI currentStateText;
    [SerializeField] private TextMeshProUGUI currentPlayerStateText;
    [SerializeField] private TextMeshProUGUI currentEnemyStateText;

    [Header("MainMenuUI")]
    [SerializeField] private RectTransform mainMenu;

    [Header("MenuInGameUI")]
    [SerializeField] private RectTransform pauseMenu;
    [SerializeField] private RectTransform gameOverUI;
    [SerializeField] private RectTransform gameUI;
    [SerializeField] private Button pauseButton;

    [Header("GameOverUI")]
    [SerializeField] private TextMeshProUGUI finalCoinText;
    [SerializeField] private TextMeshProUGUI finalDistanceText;

    [Header("Listen to Events")]
    [SerializeField] private StringEventChannelSO setWarningEvent;


    private void OnEnable()
    {
        playerData.OnDataChanged += UpdateUI;
        gameStateSO.OnGameStateChanged += HandleGameStateUI;
        gameStateSO.OnPlayerStateChanged += HandlePlayerState;
        gameStateSO.OnEnemyStateChanged += HandleEnemyState;

        setWarningEvent.OnEventRaised += HandleWarning;
    }
    private void OnDisable()
    {
        playerData.OnDataChanged -= UpdateUI;
        gameStateSO.OnGameStateChanged -= HandleGameStateUI;
        gameStateSO.OnPlayerStateChanged -= HandlePlayerState;
        gameStateSO.OnEnemyStateChanged -= HandleEnemyState;

        setWarningEvent.OnEventRaised -= HandleWarning;
    }
    private void Start()
    {
        gameUI.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
    private void HandleWarning(string text)
    {
        warningText.SetText(text);
        warningText.gameObject.SetActive(true);

        StartCoroutine(Warning());
    }
    private IEnumerator Warning()
    {
        yield return new WaitForSeconds(3f);
        warningText.gameObject.SetActive(false);
    }
    private void HandlePlayerState(PlayerState playerState)
    {
        currentPlayerStateText.SetText("Player: " + gameStateSO.CurrentPlayerState.ToString());
    }
    private void HandleEnemyState(EnemyState enemyState)
    {
        currentEnemyStateText.SetText("Enemy: " + gameStateSO.CurrentEnemyState.ToString());
    }
    private void HandleGameStateUI(GameState state)
    {
        currentStateText.SetText("State: " + gameStateSO.CurrentGameState.ToString());

        if (state == GameState.GameOver) GameOver();
        else if (state == GameState.Pause) pauseMenu.gameObject.SetActive(true);
        else if (state == GameState.Resume) pauseMenu.gameObject.SetActive(false);
    }
    private void UpdateUI()
    {
        coinText.SetText(playerData.currentCoin.ToString());
        distanceText.SetText(playerData.currentDistance.ToString("F1") + "m");
    }
    public void StartGame()
    {
        gameOverUI.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        StartCoroutine(StartCountdown());
    }
    public void PauseGame()
    {
        gameUI.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
        gameStateSO.SetGameState(GameState.Pause);
    }
    public void ResumeGame()
    {
        gameUI.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        gameStateSO.SetGameState(GameState.Resume);
    }
    public void RestartGame()
    {
        gameStateSO.SetGameState(GameState.Restart);
        gameOverUI.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
    private void GameOver()
    {
        finalCoinText.SetText(playerData.currentCoin.ToString() + " Chese");
        finalDistanceText.SetText(playerData.currentDistance.ToString("F1") + " m");

        gameUI.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
    }
    private IEnumerator StartCountdown()
    {
        for (int i = (int)countdownTime; i > 0; i--)
        {
            countdownText.text = i.ToString();
            countdownText.transform.localScale = Vector3.one * 2f; 
            countdownText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "Go!";
        countdownText.transform.localScale = Vector3.one * 2f;
        countdownText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1f);

        countdownText.SetText("");
        gameUI.gameObject.SetActive(true);
        gameStateSO.SetGameState(GameState.Start);
    }
}
