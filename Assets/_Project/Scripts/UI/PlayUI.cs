using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public TMP_Text dashText;
    public TMP_Text gameOverScoreText;
    public Button playAgainButton;
    public Button mainMenuButton;
    public GameObject gameplayUI;
    public GameObject gameOverUI;
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
        GameEvents.OnPickupCollected += OnPickup;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreChanged -= OnScoreChanged;
        GameEvents.OnPlayerDamaged -= OnPlayerDamaged;
        GameEvents.OnDashUsed -= OnDashUsed;
        GameEvents.OnPlayerDied -= OnPlayerDied;
        GameEvents.OnPickupCollected -= OnPickup;
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

    private void OnPickup(string name)
    {

    }
}
