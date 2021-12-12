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
    [SerializeField] protected float progressAdd = 0.3f;
    [SerializeField] protected float progressReduce = 0.1f;
    public bool shouldUpdate()
    {
        return !isFinished;
    }
    public virtual void  startMinigame()
    {
        isFinished = false;
        LogController.Instance.addLog(startString);
        progress = 0;

        progressBar.fillAmount = progress;

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

        isFinished = true;
        LogController.Instance.addLog(endString);
        MinigameManager.Instance.currentGameFinished(this);
    }

    public virtual void increaseDifficulty()
    {

    }
}