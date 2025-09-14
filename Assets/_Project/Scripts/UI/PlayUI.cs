using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text dashText;
    [SerializeField] TMP_Text updateText;
    [SerializeField] TMP_Text gameOverScoreText;
    [SerializeField] Button playAgainButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject gameOverUI;
    private int score = 0;


    void Start()
    {
        playAgainButton.onClick.AddListener(SceneLoader.Instance.ReloadScene);
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.LoadScene(0);
            Time.timeScale = 1;
        });
    }

    private void OnEnable()
    {
        GameEvents.OnScoreChanged += OnScoreChanged;
        GameEvents.OnPlayerDamaged += OnPlayerDamaged;
        GameEvents.OnDashUsed += OnDashUsed;
        GameEvents.OnPlayerDied += OnPlayerDied;
        GameEvents.OnGameUpdate += OnGameUpdate;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreChanged -= OnScoreChanged;
        GameEvents.OnPlayerDamaged -= OnPlayerDamaged;
        GameEvents.OnDashUsed -= OnDashUsed;
        GameEvents.OnPlayerDied -= OnPlayerDied;
        GameEvents.OnGameUpdate -= OnGameUpdate;
    }

    private void OnScoreChanged(int deltaOrOne)
    {
        score += deltaOrOne;
        if (scoreText) scoreText.text = $"Score: {score}";
    }

    private void OnPlayerDamaged(int hp)
    {
        if (healthText) healthText.text = $"HP: {hp}";
    }

    private void OnDashUsed(float cooldownRemaining)
    {
        if (dashText) dashText.text = cooldownRemaining > 0 ? $"Dash CD: {cooldownRemaining:F1}s" : "Dash Ready";
    }

    private void OnPlayerDied()
    {
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(true);
        if (gameOverScoreText) gameOverScoreText.text = $"Score: {score}";
    }

    private void OnGameUpdate(string name)
    {
        if (updateText) updateText.text = $"{name}";
        CancelInvoke(nameof(DisableUpdateText));
        Invoke(nameof(DisableUpdateText), 3);
    }

    private void DisableUpdateText()
    {
        if (updateText) updateText.text = "";
    }
}
