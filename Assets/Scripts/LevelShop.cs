using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelShop : MonoBehaviour
{
    [System.Serializable]
    public class LevelItem
    {
        public GameObject lockObject; // Объект с замком
        public Button startButton; // Кнопка старта
        public Button buyButton; // Кнопка покупки
        public TextMeshProUGUI priceText; // Текст с ценой
        public int price; // Цена уровня
    }

    public LevelItem[] levels; // Массив уровней (9 штук)
    public Text coinsText; // Текст с балансом монет

    private int coins;
    private SceneFader _sceneFader;
    private SettingsController _settingsController;

    private void Start()
    {
        _settingsController = GetComponent<SettingsController>();
        _sceneFader = GetComponent<SceneFader>();
        // Даем 10 000 монет, если их нет в PlayerPrefs
        if (!PlayerPrefs.HasKey("Coins"))
        {
            PlayerPrefs.SetInt("Coins", 0);
            PlayerPrefs.Save();
        }

        coins = PlayerPrefs.GetInt("Coins"); // Загружаем баланс
        UpdateCoinsText();

        // Первый уровень всегда куплен
        if (!PlayerPrefs.HasKey("LevelUnlocked_0"))
        {
            PlayerPrefs.SetInt("LevelUnlocked_0", 1);
            PlayerPrefs.Save();
        }

        // Проверяем, какие уровни куплены
        for (int i = 0; i < levels.Length; i++)
        {
            bool isUnlocked = PlayerPrefs.GetInt($"LevelUnlocked_{i}", 0) == 1;

            levels[i].lockObject.SetActive(!isUnlocked); // Если куплен — выключаем замок
            levels[i].startButton.gameObject.SetActive(isUnlocked); // Если куплен — включаем старт
            levels[i].priceText.text = levels[i].price.ToString(); // Устанавливаем цену в текст

            int levelIndex = i; // Локальная переменная, чтобы избежать замыкания
            levels[i].buyButton.onClick.AddListener(() => BuyLevel(levelIndex));
            levels[i].startButton.onClick.AddListener(() => StartLevel(levelIndex));
        }
    }

    private void BuyLevel(int index)
    {
        // Проверяем, куплен ли уже уровень
        if (PlayerPrefs.GetInt($"LevelUnlocked_{index}", 0) == 1)
            return;

        int price = levels[index].price;

        // Проверяем, хватает ли монет
        if (coins >= price)
        {
            _settingsController.PlayBuySound();
            coins -= price; // Вычитаем монеты
            PlayerPrefs.SetInt("Coins", coins); // Сохраняем баланс
            PlayerPrefs.SetInt($"LevelUnlocked_{index}", 1); // Отмечаем уровень как купленный
            PlayerPrefs.Save();

            levels[index].lockObject.SetActive(false); // Отключаем замок
            levels[index].startButton.gameObject.SetActive(true); // Включаем кнопку старта
            UpdateCoinsText(); // Обновляем баланс в UI
        }
    }

    private void StartLevel(int index)
    {
        PlayerPrefs.SetInt("LevelCurrent", index);
        _sceneFader.LoadSceneWithFade("game");
    }

    private void UpdateCoinsText()
    {
        coinsText.text = coins.ToString(); // Обновляем текст с балансом
    }
}
