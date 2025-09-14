using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    void Start()
    {
        _startButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.LoadScene(1);
            Time.timeScale = 1;
        });
    }
}
