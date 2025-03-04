using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null;
    public TextMeshProUGUI time;
    public TextMeshProUGUI moves;
    public GameObject winScreen;
    public TextMeshProUGUI winTime;
    public TextMeshProUGUI winMoves;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private GameAudioController _gameAudioController;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Update() {
        int minutes = Mathf.FloorToInt(ScoreManager.instance.GetTimer() / 60);
        int seconds = Mathf.FloorToInt(ScoreManager.instance.GetTimer() % 60);
        time.text = minutes > 9 ? $"{minutes:00}:{seconds:00}" : $"{minutes}:{seconds:00}";

    }

    public void ShowWinScreen() {
        int money = PlayerPrefs.GetInt("Coins", 0);
        money += 600;
        PlayerPrefs.SetInt("Coins", money);
        int minutes = Mathf.FloorToInt(ScoreManager.instance.GetTimer() / 60);
        int seconds = Mathf.FloorToInt(ScoreManager.instance.GetTimer() % 60);
        winTime.text = minutes > 9 ? $"{minutes:00}:{seconds:00}" : $"{minutes}:{seconds:00}";
        winScreen.transform.DOLocalMoveX(0, 1f);
        Debug.Log("Win!");
        _scoreManager.SaveNewTimeScore();
        _gameAudioController.DisableMusic();
        _gameAudioController.PlayWinSound();
    }

    public void HideWinScreen() {
        winScreen.transform.DOLocalMoveX(-1200, 1f);
    }
}
