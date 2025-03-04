using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BlockEvents : EventTrigger {

    public UnityEvent gameWon;
    public UnityEvent gameStarted;

    bool firstMove = false;
    float movementResistance = 60;
    enum MovementType { LEFT = 0, RIGHT, DOWN, UP };
    float onBeginX;
    float onBeginY;

    void Awake() {
        gameWon.AddListener(ScoreManager.instance.TimerStop);
        gameStarted.AddListener(ScoreManager.instance.TimerStart);
        gameWon.AddListener(ScoreManager.instance.GameWon);
        gameWon.AddListener(UIManager.instance.ShowWinScreen);
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        onBeginX = eventData.position.x;
        onBeginY = eventData.position.y;
    }

    public override void OnEndDrag(PointerEventData eventData) {
        CheckTable(CheckMovement(onBeginX, onBeginY, eventData.position.x, eventData.position.y));
    }

    void CheckTable(MovementType movement) {
        for(int i = 0; i < Spawner.instance.gameSize; i++) {
            for (int j = 0; j < Spawner.instance.gameSize; j++) {
                if(Spawner.instance.instances[i,j].spawnedBlock == this.gameObject) {
                    switch (movement) {
                        case MovementType.DOWN:
                            if(i + 1 < Spawner.instance.gameSize && Spawner.instance.instances[i+1, j].spawnedBlock == null) {
                                this.GetComponent<RectTransform>().DOMoveY(Spawner.instance.instances[i + 1, j].yValue, 0.2f);
                                Spawner.instance.instances[i + 1, j].spawnedBlock = Spawner.instance.instances[i,j].spawnedBlock;
                                Spawner.instance.instances[i, j].spawnedBlock = null;
                                int temp = Spawner.instance.instances[i, j].GetNumber();
                                Spawner.instance.instances[i, j].SetNumber(Spawner.instance.instances[i + 1, j].GetNumber());
                                Spawner.instance.instances[i + 1, j].SetNumber(temp);
                                ScoreManager.instance.AddMove();
                            }
                            break;
                        case MovementType.UP:
                            if (i - 1 > -1 && Spawner.instance.instances[i - 1, j].spawnedBlock == null) {
                                this.GetComponent<RectTransform>().DOMoveY(Spawner.instance.instances[i - 1, j].yValue, 0.2f);
                                Spawner.instance.instances[i - 1, j].spawnedBlock = Spawner.instance.instances[i, j].spawnedBlock;
                                Spawner.instance.instances[i, j].spawnedBlock = null;
                                int temp = Spawner.instance.instances[i, j].GetNumber();
                                Spawner.instance.instances[i, j].SetNumber(Spawner.instance.instances[i - 1, j].GetNumber());
                                Spawner.instance.instances[i - 1, j].SetNumber(temp);
                                ScoreManager.instance.AddMove();
                            }
                            break;
                        case MovementType.LEFT:
                            if (j - 1 > -1 && Spawner.instance.instances[i, j - 1].spawnedBlock == null) {
                                this.GetComponent<RectTransform>().DOMoveX(Spawner.instance.instances[i, j - 1].xValue, 0.2f);
                                Spawner.instance.instances[i, j - 1].spawnedBlock = Spawner.instance.instances[i, j].spawnedBlock;
                                Spawner.instance.instances[i, j].spawnedBlock = null;
                                int temp = Spawner.instance.instances[i, j].GetNumber();
                                Spawner.instance.instances[i, j].SetNumber(Spawner.instance.instances[i, j - 1].GetNumber());
                                Spawner.instance.instances[i, j - 1].SetNumber(temp);
                                ScoreManager.instance.AddMove();
                            }
                            break;
                        case MovementType.RIGHT:
                            if (j + 1 < Spawner.instance.gameSize && Spawner.instance.instances[i, j + 1].spawnedBlock == null) {
                                this.GetComponent<RectTransform>().DOMoveX(Spawner.instance.instances[i, j + 1].xValue, 0.2f);
                                Spawner.instance.instances[i, j + 1].spawnedBlock = Spawner.instance.instances[i, j].spawnedBlock;
                                Spawner.instance.instances[i, j].spawnedBlock = null;
                                int temp = Spawner.instance.instances[i, j].GetNumber();
                                Spawner.instance.instances[i, j].SetNumber(Spawner.instance.instances[i, j + 1].GetNumber());
                                Spawner.instance.instances[i, j + 1].SetNumber(temp);
                                ScoreManager.instance.AddMove();
                            }
                            break;
                        default: {
                                if (i - 1 > -1 && Spawner.instance.instances[i - 1, j].spawnedBlock == null) {
                                    this.GetComponent<RectTransform>().DOMoveY(Spawner.instance.instances[i - 1, j].yValue, 0.2f);
                                    Spawner.instance.instances[i - 1, j].spawnedBlock = Spawner.instance.instances[i, j].spawnedBlock;
                                    Spawner.instance.instances[i, j].spawnedBlock = null;
                                    int temp = Spawner.instance.instances[i, j].GetNumber();
                                    Spawner.instance.instances[i, j].SetNumber(Spawner.instance.instances[i - 1, j].GetNumber());
                                    Spawner.instance.instances[i - 1, j].SetNumber(temp);
                                    ScoreManager.instance.AddMove();
                                }
                            } break;
                    }
                    if (!firstMove) {
                        firstMove = true;
                        gameStarted.Invoke();
                    }
                }
            }
        }
        CheckForGameWin();
    }

    void CheckForGameWin() {
        bool winGame = true;
        for (int i = 0; i < Spawner.instance.gameSize; i++) {
            for (int j = 0; j < Spawner.instance.gameSize; j++) {
                if (Spawner.instance.instances[i, j].GetNumber() != (j + 1 + i * Spawner.instance.gameSize)) {
                    winGame = false;
                }
            }
        }
        if (winGame) {
            gameWon.Invoke();
            firstMove = false;
        }
    }

    MovementType CheckMovement(float startX, float startY, float endX, float endY) {
        if(startX > endX + movementResistance) {
            return MovementType.LEFT;
        } else if (endX > startX + movementResistance) {
            return MovementType.RIGHT;
        } else if (startY > endY + movementResistance) {
            return MovementType.DOWN;
        } else {
            return MovementType.UP;
        }
    }
}
