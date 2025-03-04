using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Block{
    public float xValue;
    public float yValue;
    int number;
    public GameObject spawnedBlock;
    TextMeshProUGUI textNumber;
    Sprite _sprite;

    public Block(float newxValue,float newyValue,GameObject newspawnedBlock,TextMeshProUGUI newtextNumber, Sprite spritePart) {
        xValue = newxValue;
        yValue = newyValue;
        spawnedBlock = newspawnedBlock;
        textNumber = newtextNumber;
        _sprite = spritePart;
    }

    public Block BlockStartValue(float newxValue, float newyValue, GameObject newspawnedBlock, TextMeshProUGUI newtextNumber, Sprite spritePart) {
        xValue = newxValue;
        yValue = newyValue;
        spawnedBlock = newspawnedBlock;
        textNumber = newtextNumber;
        _sprite = spritePart;
        return this;
    }

    public void SetTextNumber(int newNumber) {
        textNumber.text = newNumber.ToString();
    }

    public void SetNumber(int newNumber) {
        number = newNumber;
    }

    public void SetSpawnedBlock(GameObject newSpawnedBlock) {
        spawnedBlock = newSpawnedBlock;
    }

    public Sprite GetSprite()
    {
        return spawnedBlock.GetComponent<Image>().sprite;
    }

    public void SetSprite(Sprite sprite)
    {
        spawnedBlock.GetComponent<Image>().sprite = sprite;
    }


    public int GetNumber() { return number; }
    public TextMeshProUGUI GetTextNumber() { return textNumber; }
    public GameObject GetSpawnedBlock() { return spawnedBlock; }
}
