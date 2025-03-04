using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager instance = null;

    public bool timerStart = false;
    public bool timeHighscores = true;
    int sizeOfHighscores = 5;
    public GameObject textPrefab;
    public GameObject timeHolder;
    public GameObject movesHolder;

    int moves = 0;
    float timer = 0;
    List<float> timeScore;
    List<GameObject> spawnedScores;

	void Start () {
		if(instance == null) {
            instance = this;
        }
        sizeOfHighscores = 5;
        timeScore = new List<float>();
        spawnedScores = new List<GameObject>();
        timeScore = GetTimeListFromPrefs();
	}

    void Update() {
        if (timerStart) {
            timer += Time.deltaTime;
        }    
    }

    public void TimerStart() {
        timerStart = true;
    }
    
    public void TimerStop() {
        timerStart = false;
    }
    
    public void AddMove() {
        moves++;
    }

    public int GetMoves() {
        return moves;
    }
    
    public float GetTimer() {
        return Mathf.Round(timer);
    }

    public void RestartScore() {
        timerStart = false;
        timer = 0;
        moves = 0;
    }

    public void ChangeHighscore(bool isTimeScoreOn) {
        timeHighscores = isTimeScoreOn;
        ClearHighscore();
    }

    public void SaveNewTimeScore(float time) {
        timeScore.Add(time);
        timeScore.Sort();
        if (timeScore.Count > sizeOfHighscores) {
            timeScore.RemoveAt(timeScore.Count - 1);
        }
        SaveTimeListToPrefs();
        for(int i = 0; i < spawnedScores.Count; i++) {
            Destroy(spawnedScores[i]);
        }
        spawnedScores.Clear();
        timeScore.Clear();
        timeScore = GetTimeListFromPrefs();
    }

    public void SaveTimeListToPrefs() {
       for(int i = 0; i < timeScore.Count; i++) {
            PlayerPrefs.SetFloat("ScoreTime" + Spawner.instance.gameSize + i, timeScore[i]);
        }
    }

    public void ClearHighscore() {
        for (int i = 0; i < spawnedScores.Count; i++) {
            Destroy(spawnedScores[i]);
        }
        spawnedScores.Clear();
        timeScore.Clear();
        if (timeHighscores) {
            timeScore = GetTimeListFromPrefs();
        } else {
            timeScore = GetMovesListFromPrefs();
        }
    }

    public List<float> GetTimeListFromPrefs() {
        int i = 0;
        float tempValue;
        List<float> newList = new List<float>();
        while(PlayerPrefs.HasKey("ScoreTime" + Spawner.instance.gameSize + i)) {
            tempValue = PlayerPrefs.GetFloat("ScoreTime" + Spawner.instance.gameSize + i);
            newList.Add(tempValue);
            spawnedScores.Add(Instantiate(textPrefab, timeHolder.transform));
            spawnedScores[i].GetComponent<TextMeshProUGUI>().text = string.Concat(System.Math.Round(tempValue,2),"s");
            i++;
        }
        return newList;
    }

    public List<float> GetMovesListFromPrefs() {
        int i = 0;
        float tempValue;
        List<float> newList = new List<float>();
        while (PlayerPrefs.HasKey("ScoreMoves" + Spawner.instance.gameSize + i)) {
            tempValue = PlayerPrefs.GetFloat("ScoreMoves" + Spawner.instance.gameSize + i);
            newList.Add(tempValue);
            spawnedScores.Add(Instantiate(textPrefab, timeHolder.transform));
            spawnedScores[i].GetComponent<TextMeshProUGUI>().text = tempValue.ToString();
            i++;
        }
        return newList;
    }

    public void SaveMovesListToPrefs() {
        for (int i = 0; i < timeScore.Count; i++) {
            PlayerPrefs.SetFloat("ScoreMoves" + Spawner.instance.gameSize + i, timeScore[i]);
        }
    }

    public void SaveNewMovesScore(float time) {
        timeScore.Add(time);
        timeScore.Sort();
        if (timeScore.Count > sizeOfHighscores) {
            timeScore.RemoveAt(timeScore.Count - 1);
        }
        SaveMovesListToPrefs();
        for (int i = 0; i < spawnedScores.Count; i++) {
            Destroy(spawnedScores[i]);
        }
        spawnedScores.Clear();
        timeScore.Clear();
        timeScore = GetMovesListFromPrefs();
    }

    public bool IsBetterScore(float time) {
        bool temp = false;
        for (int i = 0; i < timeScore.Count; i++) {
            if(timeScore[i] > time) {
                temp = true;
            }
        }
            if (timeScore.Count <= Spawner.instance.gameSize) {
            temp = true;
        }
        return temp;
    }

    public void GameWon() {
        timeScore = GetTimeListFromPrefs();
        if (IsBetterScore(timer)) {
            SaveNewTimeScore(timer);
        }
        timeScore = GetMovesListFromPrefs();
        if (IsBetterScore(moves)) {
            SaveNewMovesScore(moves);
        }
        ClearHighscore();
    }
}
