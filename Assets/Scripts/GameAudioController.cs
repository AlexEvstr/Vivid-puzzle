using UnityEngine;

public class GameAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _soundController;
    [SerializeField] private AudioSource _musicController;

    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _win;
    [SerializeField] private AudioClip _drag;
    private float _vibration;

    private void Start()
    {
        Vibration.Init();
        _soundController.volume = PlayerPrefs.GetFloat("SoundVolume", 1);
        _musicController.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
        _vibration = PlayerPrefs.GetFloat("VibroStatus", 1);
    }

    public void PlayClickSound()
    {
        _soundController.PlayOneShot(_click);
        if (_vibration == 1) Vibration.VibratePop();
    }

    public void PlayWinSound()
    {
        _soundController.PlayOneShot(_win);
        if (_vibration == 1) Vibration.Vibrate();
    }

    public void PlayDragSound()
    {
        _soundController.PlayOneShot(_drag);
        if (_vibration == 1) Vibration.VibratePop();
    }

    public void DisableMusic()
    {
        _musicController.Stop();
    }
}