using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    public bool timerStart = false;
    private int maxScores = 7; // Количество ячеек
    public GameObject[] scoreCells; // Ячейки для хранения лучших результатов (7 штук)

    private int currentLevel;
    private float timer = 0;
    private List<float> timeScores;
    private List<string> dateScores;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentLevel = PlayerPrefs.GetInt("LevelCurrent", 0); // Получаем текущий уровень
        timeScores = new List<float>();
        dateScores = new List<string>();

        LoadScores(); // Загружаем сохраненные результаты
        UpdateScoreDisplay(); // Обновляем отображение
    }

    void Update()
    {
        if (timerStart)
        {
            timer += Time.deltaTime;
        }
    }

    public void TimerStart()
    {
        timerStart = true;
    }

    public void TimerStop()
    {
        timerStart = false;
    }

    public void RestartScore()
    {
        timerStart = false;
        timer = 0;
    }

    public float GetTimer()
    {
        return Mathf.Round(timer);
    }

    public void SaveNewTimeScore()
    {
        float roundedTime = Mathf.Round(timer);
        string formattedDate = DateTime.Now.ToString("dd.MM.yyyy");

        timeScores.Add(roundedTime);
        dateScores.Add(formattedDate);

        // Сортировка по времени (лучшее время — наверху)
        for (int i = 0; i < timeScores.Count - 1; i++)
        {
            for (int j = i + 1; j < timeScores.Count; j++)
            {
                if (timeScores[i] > timeScores[j])
                {
                    (timeScores[i], timeScores[j]) = (timeScores[j], timeScores[i]);
                    (dateScores[i], dateScores[j]) = (dateScores[j], dateScores[i]);
                }
            }
        }

        // Ограничиваем список 7 результатами
        while (timeScores.Count > maxScores)
        {
            timeScores.RemoveAt(timeScores.Count - 1);
            dateScores.RemoveAt(dateScores.Count - 1);
        }

        SaveScores();
        UpdateScoreDisplay();
    }

    private void SaveScores()
    {
        for (int i = 0; i < timeScores.Count; i++)
        {
            PlayerPrefs.SetFloat($"Level_{currentLevel}_ScoreTime_{i}", timeScores[i]);
            PlayerPrefs.SetString($"Level_{currentLevel}_ScoreDate_{i}", dateScores[i]);
        }
        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        timeScores.Clear();
        dateScores.Clear();

        for (int i = 0; i < maxScores; i++)
        {
            if (PlayerPrefs.HasKey($"Level_{currentLevel}_ScoreTime_{i}"))
            {
                timeScores.Add(PlayerPrefs.GetFloat($"Level_{currentLevel}_ScoreTime_{i}"));
                dateScores.Add(PlayerPrefs.GetString($"Level_{currentLevel}_ScoreDate_{i}"));
            }
        }
    }

    private void UpdateScoreDisplay()
    {
        for (int i = 0; i < maxScores; i++)
        {
            Transform timeText = scoreCells[i].transform.GetChild(0);
            Transform dateText = scoreCells[i].transform.GetChild(1);

            if (i < timeScores.Count)
            {
                timeText.GetComponent<TextMeshProUGUI>().text = FormatTime(timeScores[i]);
                dateText.GetComponent<TextMeshProUGUI>().text = dateScores[i];
            }
            else
            {
                timeText.GetComponent<TextMeshProUGUI>().text = "0:00";
                dateText.GetComponent<TextMeshProUGUI>().text = "---";
            }
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return minutes > 9 ? $"{minutes:00}:{seconds:00}" : $"{minutes}:{seconds:00}";
    }

    public void ResetScores()
    {
        for (int i = 0; i < maxScores; i++)
        {
            PlayerPrefs.DeleteKey($"Level_{currentLevel}_ScoreTime_{i}");
            PlayerPrefs.DeleteKey($"Level_{currentLevel}_ScoreDate_{i}");
        }
        PlayerPrefs.Save();

        LoadScores();
        UpdateScoreDisplay();
    }
}
