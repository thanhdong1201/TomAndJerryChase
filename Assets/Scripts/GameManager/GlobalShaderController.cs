using UnityEngine;

public class GlobalShaderController : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private string[] shaderNames;

    [Header("Manual Control")]
    [Range(-0.005f, 0.005f)][SerializeField] private float sideway = 0f;
    [Range(-0.002f, 0.002f)][SerializeField] private float backward = 0f;

    [Header("Auto Control")]
    [SerializeField] private bool autoAdjust = true;
    [SerializeField] private float curveSpeed = 0.2f; // Tốc độ chuyển tiếp cong (càng nhỏ càng chậm)

    private float holdTime = 5f; // Thời gian giữ độ cong trước khi trở lại thẳng
    private float straightDurationMin = 5f; // Thời gian đi thẳng tối thiểu
    private float straightDurationMax = 10f; // Thời gian đi thẳng tối đa

    private float maxCurve = 0.005f;
    private float minCurve = -0.005f;
    private float targetCurve = 0f;
    private float changeTime;
    private bool isCurving = false;

    private void OnValidate()
    {
        ApplyShaderValues();
    }
    private void Start()
    {
        changeTime = Time.time + Random.Range(straightDurationMin, straightDurationMax);
    }

    private void Update()
    {
        if(gameStateSO.CurrentGameState == GameState.GameOver || gameStateSO.CurrentGameState == GameState.None || gameStateSO.CurrentGameState == GameState.Restart) return;

        if (autoAdjust)
        {
            UpdateCurveOverTime();
        }

        ApplyShaderValues();
    }

    private void UpdateCurveOverTime()
    {
        if (!isCurving && Time.time >= changeTime)
        {
            isCurving = true;
            targetCurve = Random.value > 0.5f ? maxCurve : minCurve;
            Invoke(nameof(ResetCurve), holdTime);
        }

        sideway = Mathf.Lerp(sideway, targetCurve, Time.deltaTime * curveSpeed);
    }
    private void ResetCurve()
    {
        targetCurve = 0f;
        isCurving = false;
        changeTime = Time.time + Random.Range(straightDurationMin, straightDurationMax);
    }
    private void ApplyShaderValues()
    {
        foreach (string sdName in shaderNames)
        {
            Shader sd = Shader.Find(sdName);
            if (sd == null) return;

            Material[] materials = Resources.FindObjectsOfTypeAll<Material>();

            foreach (Material mat in materials)
            {
                if (mat.shader == sd)
                {
                    mat.SetFloat("_Sideway", sideway);
                    mat.SetFloat("_Backward", backward);
                }
            }
        }
    }
}
