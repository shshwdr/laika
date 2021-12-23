using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame:MonoBehaviour
{
    public bool isFinished = true;
    public Image progressBar;
    public float progress;
    public string startString;
    public string endString;
    public KeyAction keyAction;
    public string keyArrow { get {
            return KeyBindingManager.keyDict[keyAction].ToString();
        } }
    public string gameName;
    [SerializeField] protected float progressAdd = 0.3f;
    [SerializeField] protected float progressReduce = 0.1f;

    public bool canFinish()
    {
        return progress >= 0.99f;
    }
    public bool shouldUpdate()
    {
        return !isFinished;
    }
    public virtual void  startMinigame()
    {
        isFinished = false;
        progress = 0;

        progressBar.fillAmount = progress;
        //MinigameManager.Instance.startMiniGame(this);

    }

    public void updateProgress()
    {
        if (progress >= 1)
        {
            finishedGame();
            //StartCoroutine(waitAndRestart());
        }
        progress = Mathf.Clamp(progress, 0, 1);
        progressBar.fillAmount = progress;
    }
    public virtual void finishedGame()
    {

        MinigameManager.Instance.currentGameFinished(this);
    }

    public virtual void realFinishedGame()
    {

        isFinished = true;
    }

    public virtual void increaseDifficulty()
    {

    }
}