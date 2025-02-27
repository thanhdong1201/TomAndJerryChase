using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public enum UIPanel
{
    MainMenu,
    Gameplay,
    Pause,
    GameOver
}
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private PlayerDataSO playerData;

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI finalCoinText;
    [SerializeField] private TextMeshProUGUI finalDistanceText;

    [Header("UI Components")]
    [SerializeField] private UICountdown uiCountdown;

    [Header("Listen from Events")]
    [SerializeField] private StringEventChannelSO setWarningEvent;


    private Dictionary<UIPanel, GameObject> uiPanels;

    private void Awake()
    {
        InitializeUIPanels();
    }
    private void OnEnable()
    {
        playerData.OnDataChanged += HandleUpdateHUD;
        gameStateSO.OnGameStateChanged += HandleGameStateUI;
        setWarningEvent.OnEventRaised += HandleWarning;
    }
    private void OnDisable()
    {
        playerData.OnDataChanged -= HandleUpdateHUD;
        gameStateSO.OnGameStateChanged -= HandleGameStateUI;
        setWarningEvent.OnEventRaised -= HandleWarning;
    }
    private void HandleGameStateUI(GameState state)
    {
        if (state == GameState.GameOver) GameOver();
    }
    private void HandleUpdateHUD()
    {
        coinText.SetText(playerData.currentCoin.ToString());

        if(playerData.currentDistance >= 0f)
        {
            distanceText.SetText(playerData.currentDistance.ToString("F1") + "m");
        }
        else
        {
            distanceText.SetText("");
        }
     
    }
    private void HandleWarning(string text)
    {
        warningText.SetText(text);
        warningText.gameObject.SetActive(true);
        StartCoroutine(DisableAfterSeconds(warningText.gameObject, 3f));
    }
    private void Start()
    {
        ShowUI(UIPanel.MainMenu);
    }
    private void InitializeUIPanels()
    {
        uiPanels = new Dictionary<UIPanel, GameObject>
        {
            { UIPanel.MainMenu, mainMenuUI },
            { UIPanel.Gameplay, gameplayUI },
            { UIPanel.Pause, pauseMenuUI },
            { UIPanel.GameOver, gameOverUI }
        };

        foreach (GameObject panel in uiPanels.Values)
        {
            if (panel != null) panel.SetActive(false);
        }
    }
    private void ShowUI(UIPanel panel)
    {
        foreach (var key in uiPanels.Keys)
        {
            uiPanels[key]?.SetActive(key == panel);        
        }
    }
    public void StartGame()
    {
        ShowUI(UIPanel.Gameplay);
        uiCountdown.Countdown();
    }
    public void PauseGame()
    {
        ShowUI(UIPanel.Pause);
        gameStateSO.SetGameState(GameState.Pause);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        ShowUI(UIPanel.Gameplay);
        gameStateSO.SetGameState(GameState.Resume);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameStateSO.SetGameState(GameState.Restart);
        ShowUI(UIPanel.MainMenu);
    }
    private void GameOver()
    {
        finalCoinText.SetText($"{playerData.currentCoin} Cheese");
        finalDistanceText.SetText($"{playerData.currentDistance:F1} m");
        ShowUI(UIPanel.GameOver);
    }
    private IEnumerator DisableAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }
}

