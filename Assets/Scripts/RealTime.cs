using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RealTime : MonoBehaviour {

    public TextMeshProUGUI timeText;

    void Start() {
        StartCoroutine(UpdateTime());
    }

    IEnumerator UpdateTime() {
        while (true) {
            System.DateTime today = System.DateTime.Now;
            timeText.text = today.ToString("HH:mm");
            yield return new WaitForSeconds(1f);
        }
    }
}
