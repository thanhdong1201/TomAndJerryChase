using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class UICountdown : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private TextMeshProUGUI countdownText;
    private float countdownTime = 3f;

    public void Countdown()
    {
        StartCoroutine(StartCountdown());
    }
    private IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

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
        countdownText.gameObject.SetActive(false);
        gameStateSO.SetGameState(GameState.Start);
    }
}
