using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Used to spawn game blocks inside canvas it is attached to
public class Spawner : MonoBehaviour {

    public GameObject blockPrefab;
    public Canvas canvas;
    private float minSpacing = -10;
    public int gameSize;
    public static Spawner instance;

    RectTransform rectTransform;
    RectTransform prefabRectTransform;
    GameObject tempGameObject;
    System.Random randomNumber = new System.Random();
    int shuffleIterations = 50;
    float canvasTopLeftY = 0;
    float canvasTopLeftX = 0;
    float canvasWidth = 0;
    float canvasHeight = 0;
    float lastXvalue = 0;
    float lastYvalue = 0;
    float sizeOfBlock = 0;
    public Block[,] instances;
    public int[,] numbers;
    [SerializeField] private List<LevelSprites> _imagePartsPerLevel = new List<LevelSprites>();
    private List<Sprite> currentImageParts;
    [SerializeField] private Image _winImage;
    [SerializeField] private Image _targetImage;
    [SerializeField] private Sprite[] _fullImages;

    void Awake() {
        if(instance == null) {
            instance = this;
        }

        int currentLevel = PlayerPrefs.GetInt("LevelCurrent", 0);
        if (currentLevel < _imagePartsPerLevel.Count)
        {
            currentImageParts = _imagePartsPerLevel[currentLevel].sprites;
        }
        if (currentLevel < 6) gameSize = 3;
        else gameSize = 4;
        _winImage.sprite = _fullImages[currentLevel];
        _targetImage.sprite = _fullImages[currentLevel];
        SetDefaultValues();
    }
    void Start () {
        SpawnBlocks();
        GenerateNumbers();
        SetNumbers();
        DeleteLastNumber();
    }

    void SpawnBlocks() {
        lastXvalue = canvasTopLeftX + minSpacing;
        lastYvalue = canvasTopLeftY - minSpacing;
        int row = 0;
        int column = 0;
        for (int i = 0; i < (gameSize * gameSize); i++) {
            instances[row, column] = new Block(lastXvalue, lastYvalue,
            tempGameObject = Instantiate(blockPrefab, new Vector3(lastXvalue, lastYvalue, 0),
            Quaternion.identity, this.transform), tempGameObject.GetComponentInChildren<TextMeshProUGUI>(), tempGameObject.GetComponent<Image>().sprite);
            if (i < (gameSize * gameSize)-1) tempGameObject.GetComponent<Image>().sprite = currentImageParts[i];
            lastXvalue = lastXvalue + minSpacing + sizeOfBlock;
            column++;
            
            if (lastXvalue >= (canvasTopLeftX + canvasWidth)) {
                row++;
                column = 0;
                lastXvalue = canvasTopLeftX + minSpacing;
                lastYvalue = (lastYvalue - sizeOfBlock - minSpacing);
            }
        }
    }

    void ShuffleNumbers()
    {
        int n, temp, l;
        Sprite tempSprite;

        for (int i = shuffleIterations; i > 0; i--)
        {
            for (int j = gameSize; j > 0;)
            {
                n = randomNumber.Next(j);
                --j;
                l = randomNumber.Next(j);

                // Перемешивание чисел
                temp = numbers[l, j];
                numbers[l, j] = numbers[n, l];
                numbers[n, l] = temp;

                // Перемешивание спрайтов
                tempSprite = instances[l, j].GetSprite();
                instances[l, j].SetSprite(instances[n, l].GetSprite());
                instances[n, l].SetSprite(tempSprite);
            }
        }
    }


    void GenerateNumbers() {
        for(int j = 0; j < gameSize; j++) {
            for(int i = 0; i < gameSize; i++) {
                numbers[j, i] = j * gameSize + i + 1;
            }
        }
        ShuffleNumbers();
    }

    void SetNumbers() {
        for (int j = 0; j < gameSize; j++)
        {
            for (int i = 0; i < gameSize; i++)
            {
                instances[j, i].SetTextNumber(numbers[j, i]);
                instances[j, i].SetNumber(numbers[j, i]);
            }
        }
    }

    void DeleteLastNumber() {
        for (int j = 0; j < gameSize; j++)
        {
            for (int i = 0; i < gameSize; i++)
            {
                if (numbers[j, i] == gameSize * gameSize)
                {
                    Destroy(instances[j, i].GetSpawnedBlock());
                    instances[j, i].SetSpawnedBlock(null);
                }
            }
        }
    }

    
    void SetDefaultValues() {
        instances = new Block[gameSize, gameSize];
        tempGameObject = new GameObject();
        numbers = new int[gameSize, gameSize];
        rectTransform = GetComponent<RectTransform>();
        prefabRectTransform = blockPrefab.GetComponent<RectTransform>();
        canvasTopLeftY = rectTransform.position.y;
        canvasTopLeftX = rectTransform.position.x;
        canvasWidth = rectTransform.rect.width;
        canvasHeight = rectTransform.rect.height;
        sizeOfBlock = (canvasWidth - ((gameSize + 1) * minSpacing)) / gameSize;
        prefabRectTransform.sizeDelta = new Vector2(sizeOfBlock, sizeOfBlock);
    }

    void ClearGame() {
        for (int j = 0; j < gameSize; j++) {
            for (int i = 0; i < gameSize; i++) {
                Destroy(instances[j, i].GetSpawnedBlock());
                instances[j, i].SetSpawnedBlock(null);
            }
        }
    }

    public void SetGameSize(int gameSize) {
        ClearGame();
        this.gameSize = gameSize;
        SetDefaultValues();
        SpawnBlocks();
        GenerateNumbers();
        SetNumbers();
        DeleteLastNumber();
    }

    public void RestartGame() {
        ClearGame();
        SetDefaultValues();
        SpawnBlocks();
        GenerateNumbers();
        SetNumbers();
        DeleteLastNumber();
    }
}

[System.Serializable]
public class LevelSprites
{
    public List<Sprite> sprites;
}
