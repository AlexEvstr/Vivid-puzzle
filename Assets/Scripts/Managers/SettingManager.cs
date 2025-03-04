using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingManager : MonoBehaviour {

    public static SettingManager instance = null;

    public GameObject settingsCanvas;
    public RawImage imageToChange;
    public Texture settingPicture;
    public Texture backPicture;
    public Image timesImage;
    public Image movesImage;
    public Image button9Image;
    public Image button16Image;
    public Image button25Image;

    bool isActive = false;
    public Color normalColor;
    public Color pushedColor; //Change in editor

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    void Start() {
        ChangeGameSizeButtonColor();
        ChangeButtonColor();
    }

    public void ToogleSetActive() {
        if (isActive) {
            isActive = false;
            settingsCanvas.SetActive(isActive);
            imageToChange.texture = settingPicture;
        } else {
            isActive = true;
            settingsCanvas.SetActive(isActive);
            imageToChange.texture = backPicture;
        }
    }

    public void ChangeButtonColor() {
        if (ScoreManager.instance.timeHighscores) {
            timesImage.DOColor(pushedColor, 0.5f);
            movesImage.DOColor(normalColor, 0.5f);
        } else {
            movesImage.DOColor(pushedColor, 0.5f);
            timesImage.DOColor(normalColor, 0.5f);
        }
    }

    public void ChangeHighscore(bool timeHighscore) {
        ScoreManager.instance.ChangeHighscore(timeHighscore);
    }

    public void ChangeGameSizeButtonColor() {
        switch (Spawner.instance.gameSize) {
            case 3: {
                    button9Image.DOColor(pushedColor, 0.5f);
                    button16Image.DOColor(normalColor, 0.5f);
                    button25Image.DOColor(normalColor, 0.5f);
                    break;
                }
            case 4: {
                    button9Image.DOColor(normalColor, 0.5f);
                    button16Image.DOColor(pushedColor, 0.5f);
                    button25Image.DOColor(normalColor, 0.5f);
                    break;
                }
            case 5: {
                    button9Image.DOColor(normalColor, 0.5f);
                    button16Image.DOColor(normalColor, 0.5f);
                    button25Image.DOColor(pushedColor, 0.5f);
                    break;
                }
        }
    }
}
