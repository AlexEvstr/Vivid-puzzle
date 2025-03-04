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

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Update() {
        time.text = ScoreManager.instance.GetTimer().ToString();
        //moves.text = ScoreManager.instance.GetMoves().ToString();
    }

    public void ShowWinScreen() {
        int minutes = Mathf.FloorToInt(ScoreManager.instance.GetTimer() / 60);
        int seconds = Mathf.FloorToInt(ScoreManager.instance.GetTimer() % 60);
        winTime.text = minutes > 9 ? $"{minutes:00}:{seconds:00}" : $"{minutes}:{seconds:00}";
        winScreen.transform.DOLocalMoveX(0, 1f);
        Debug.Log("Win!");
        _scoreManager.SaveNewTimeScore();
    }

    public void HideWinScreen() {
        winScreen.transform.DOLocalMoveX(-1200, 1f);
    }
}
