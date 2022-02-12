using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : Singleton<MinigameManager>
{
    MiniGame[] miniGames;

   public  int dialogId = 2;

    public List<GameObject> miniGameObjects;

    List<MiniGame> currentMiniGames = new List<MiniGame>();
    Dictionary<MiniGame, bool> miniGamefinished = new Dictionary<MiniGame, bool>();
    int minigameId = 0;
     List<List<int>> minigameOrder = new List<List<int>>() { new List<int>(){ 1 },
         new List<int>(){ 0 },
          new List<int>(){ 1,0 },
           new List<int>(){ 2 },
            new List<int>(){ 2,1 }
     };
    private void Awake()
    {
        miniGames = GetComponentsInChildren<MiniGame>();
        foreach (var ob in miniGameObjects)
        {
            ob.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(waitAndRestart());
        EventPool.OptIn("finishedMovieText", restartAfterMovie);
    }

    public void startGame()
    {

    }
    void EndGame()
    {
        StartAndEndLogic.Instance.end();
    }
    public IEnumerator waitAndRestart()
    {

        if(minigameId >= minigameOrder.Count)
        {
            yield return new WaitForSeconds(2f);
            EndGame();
            yield break;
        }
        yield return new WaitForSeconds(2f);
        PixelCrushers.DialogueSystem.DialogueManager.StartConversation("MinigameStart" + dialogId);
        currentMiniGames.Clear();
        miniGamefinished.Clear();

        foreach (var id in minigameOrder[minigameId])
        {
            miniGameObjects[id].SetActive(true) ;
            currentMiniGames.Add(miniGames[id]);
            miniGamefinished[miniGames[id]] = false;
        }


        yield return new WaitForSeconds(2f);

        //if (minigameOrder[minigameId].Count > 1)
        //{

        //    LogController.Instance.addLog("You need to solve two problem at the same time");
        //}
        bool isMultiple = minigameOrder[minigameId].Count > 1;
        foreach (var id in minigameOrder[minigameId])
        {

            miniGames[id].startMinigame();
            if (!isMultiple)
            {

                LogController.Instance.addLog(miniGames[id].startString);
            }
        }
        if (isMultiple)
        {
            LogController.Instance.addLog(string.Format("You need to use {0} and {1} to control both {2} and {3} at the same time.", miniGames[minigameOrder[minigameId][0]].keyArrow, miniGames[minigameOrder[minigameId][1]].keyArrow, miniGames[minigameOrder[minigameId][0]].gameName, miniGames[minigameOrder[minigameId][1]].gameName));
        }
        MusicManager.Instance.playMiniGame();
        

    }


    void finisheAllGames()
    {

    }

    public void currentGameFinished(MiniGame game,bool forceFinish = false)
    {
        //if (miniGamefinished.ContainsKey(game))
        //{
        //    miniGamefinished[game] = true ;
        //}
        //else
        //{
        //    Debug.LogError("mini game cant finished");
        //}
        if (!forceFinish)
        {

            foreach (var g in currentMiniGames)
            {
                if (!g.canFinish())
                {
                    return;
                }
            }
        }
        //if (currentMiniGames.Count == 0)
        {

            bool isMultiple = minigameOrder[minigameId].Count > 1;

            foreach (var g in currentMiniGames)
            {
                g.realFinishedGame();
                if (!isMultiple)
                    LogController.Instance.addLog(g.endString);
            }

            if (isMultiple)
            {
                LogController.Instance.addLog(string.Format("Successfully controlled both {0} and {1} at the same time.",  miniGames[minigameOrder[minigameId][0]].gameName, miniGames[minigameOrder[minigameId][1]].gameName));
            }

            if(minigameId == minigameOrder.Count - 1)
            {

                MusicManager.Instance.playFinal();
            }
            else
            {

                MusicManager.Instance.playNormal();
            }

            if (MovieTextController.Instance.showTextLabel("poemEnd" + dialogId))
            {
            }
            else
            {

                StartCoroutine(waitAndShowEndDialogue());

            }

            minigameId++;
            //if (minigameId >= minigameOrder.Count)
            //{
            //    minigameId = 0;
            //}
        }

    }

    public IEnumerator waitAndShowEndDialogue()
    {
        yield return new WaitForSeconds(0.3f);
        PixelCrushers.DialogueSystem.DialogueManager.StartConversation("MinigameEnd" + dialogId);
        yield return new WaitForSeconds(0.3f);
        

        dialogId++;

        StartCoroutine(waitAndRestart());


    }

    public void restartAfterMovie()
    {

        StartCoroutine(waitAndShowEndDialogue());
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //foreach(var game in currentMiniGames)
            {
                currentGameFinished(currentMiniGames[0], true);
            }
        }
    }
}
