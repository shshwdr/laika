using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : Singleton<MinigameManager>
{
    MiniGame[] miniGames;

    int dialogId = 1;

    MiniGame currentMiniGame;
    int minigameId = 0;
    List<int> minigameOrder = new List<int>() { 1, 0, 2, 0, 1, 2 };

    // Start is called before the first frame update
    void Start()
    {
        miniGames = GetComponentsInChildren<MiniGame>();
        StartCoroutine(waitAndRestart());

        EventPool.OptIn("finishedMovieText", restartAfterMovie);
    }

    public IEnumerator waitAndRestart()
    {
        yield return new WaitForSeconds(2f);
        waitAndShowStartDialogue(miniGames[minigameOrder[minigameId]]);
        yield return new WaitForSeconds(2f);
        miniGames[minigameOrder[minigameId]].startMinigame();
        minigameId++;
        if(minigameId>= minigameOrder.Count)
        {
            minigameId = 0;
        }

    }

    //public void startMiniGame(MiniGame game)
    //{
    //    StartCoroutine(waitAndShowStartDialogue(game));
    //}



    public void waitAndShowStartDialogue(MiniGame game)
    {
        PixelCrushers.DialogueSystem.DialogueManager.StartConversation("MinigameStart" + dialogId);
        currentMiniGame = game;
        MusicManager.Instance.playMiniGame(); 
       // StartCoroutine(waitAndRestart());

    }

    public void currentGameFinished(MiniGame game)
    {
        MusicManager.Instance.playNormal();
        StartCoroutine(waitAndShowEndDialogue());

    }

    public IEnumerator waitAndShowEndDialogue()
    {
        yield return new WaitForSeconds(0.3f);
        PixelCrushers.DialogueSystem.DialogueManager.StartConversation("MinigameEnd" + dialogId);
        yield return new WaitForSeconds(0.3f);
        if (MovieTextController.Instance.showTextLabel("poemEnd" + dialogId))
        {
            dialogId++;
        }
        else
        {

            dialogId++;


            StartCoroutine(waitAndRestart());
        }



    }

    public void restartAfterMovie()
    {

        StartCoroutine(waitAndRestart());
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentMiniGame.finishedGame();
        }
    }
}
