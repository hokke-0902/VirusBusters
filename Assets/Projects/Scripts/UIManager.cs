using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI maxComboText;

    [Header("UI Labels")]
    [SerializeField] private string scoreLabel = "Score";
    [SerializeField] private string timeLabel = "Time";
    [SerializeField] private string comboLabel = "Max Combo";

    [Header("Countdown UI")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private string readyText = "Ready...";
    [SerializeField] private string goText = "Go！";
    [SerializeField] private float readyDisplayTime = 1.5f;
    [SerializeField] private float goDisplayTime = 1f;

    [Header("End Text UI")]
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private string endMessage = "End！";
    [SerializeField] private float endDisplayTime = 2f;

    [Header("Score Popup UI")]
    [SerializeField] private GameObject scorePopupPrefab;       // ポップアップ用プレハブ
    [SerializeField] private Transform scorePopupParent;        // Canvas上の配置先（例: Popups）
    // ★ 追加：ポップアップを表示したい位置（UIオブジェクトのRectTransform）
    [SerializeField] private RectTransform popupFixedPosition;


    private int maxCombo = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText();
        UpdateMaxComboText();
    }

    void Update()
    {
        if (timerText != null && GameManager.Instance != null)
        {
            float currentTime = GameManager.Instance.GetCurrentTime();
            timerText.text = $"{timeLabel}{Mathf.CeilToInt(currentTime)}";
        }
    }

    public void AddScore(int points)
    {
        UpdateScoreText();
    }


    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{scoreLabel}{ScoreManager.Instance.score}";
        }
    }


    public void UpdateCombo(int currentCombo)
    {
        if (currentCombo > maxCombo)
        {
            maxCombo = currentCombo;
            UpdateMaxComboText();
        }
    }

    private void UpdateMaxComboText()
    {
        if (maxComboText != null)
        {
            maxComboText.text = $"{comboLabel}: {maxCombo}";
        }
    }

    public void StartCountdown(System.Action onComplete)
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        StartCoroutine(CountdownCoroutine(onComplete));
    }

    private IEnumerator CountdownCoroutine(System.Action onComplete)
{
    if (countdownText != null)
    {
        countdownText.text = readyText;
    }

    yield return new WaitForSeconds(readyDisplayTime); // Ready... 表示

    if (countdownText != null)
    {
        countdownText.text = goText;
    }

    yield return new WaitForSeconds(goDisplayTime); // ★ Start！表示の時間を待つ ← 追加

    if (countdownText != null)
    {
        countdownText.gameObject.SetActive(false);
    }

    onComplete?.Invoke(); // ★ Start！表示が終わった後にゲーム開始！
}

    public void ShowEndText(System.Action onComplete)
    {
        if (endText != null)
        {
            endText.text = endMessage;
            endText.gameObject.SetActive(true);
        }

        StartCoroutine(EndTextCoroutine(onComplete));
    }

    private IEnumerator EndTextCoroutine(System.Action onComplete)
    {
        yield return new WaitForSeconds(endDisplayTime);

        if (endText != null)
        {
            endText.gameObject.SetActive(false);
        }

        onComplete?.Invoke();
    }
    public void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{scoreLabel}{ScoreManager.Instance.score}";
        }
    }

    public void ShowScorePopup(string text, Color color)
{
    if (scorePopupPrefab == null || scorePopupParent == null || popupFixedPosition == null) return;

    GameObject popup = Instantiate(scorePopupPrefab, scorePopupParent);

    RectTransform popupRect = popup.GetComponent<RectTransform>();
    popupRect.anchoredPosition = popupFixedPosition.anchoredPosition;

    ScorePopup popupScript = popup.GetComponent<ScorePopup>();
    if (popupScript != null)
    {
        popupScript.SetText(text, color);
    }
}




}
