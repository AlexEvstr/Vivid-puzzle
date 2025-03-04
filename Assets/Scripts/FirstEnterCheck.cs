using UnityEngine;

public class FirstEnterCheck : MonoBehaviour
{
    [SerializeField] private RectTransform _enterFrame;
    private PopupEffect _popupEffect;

    private void Start()
    {
        _popupEffect = GetComponent<PopupEffect>();
        int enter = PlayerPrefs.GetInt("FirstEnteControl", 0);
        if (enter == 0) _popupEffect.OpenWindow(_enterFrame);
    }

    public void ChangeEnterStatus()
    {
        _popupEffect.CloseWindow(_enterFrame);
        PlayerPrefs.SetInt("FirstEnteControl", 1);
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
