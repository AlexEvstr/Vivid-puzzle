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

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Update() {
        time.text = ScoreManager.instance.GetTimer().ToString();
        moves.text = ScoreManager.instance.GetMoves().ToString();
    }

    public void ShowWinScreen() {
        winTime.text = string.Concat(ScoreManager.instance.GetTimer().ToString(),"s");
        winMoves.text = ScoreManager.instance.GetMoves().ToString();
        winScreen.transform.DOLocalMoveX(0, 1f);
    }

    public void HideWinScreen() {
        winScreen.transform.DOLocalMoveX(-1200, 1f);
    }
}
