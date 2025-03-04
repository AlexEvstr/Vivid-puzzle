using UnityEngine;

public class SettingManager : MonoBehaviour {

    public static SettingManager instance = null;

    public GameObject resultsPanel;
    

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void OpenResults()
    {
        resultsPanel.SetActive(true);
    }

    public void CloseResults()
    {
        resultsPanel.SetActive(false);
    }
}