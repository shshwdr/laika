using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartbeatMiniGame : MiniGame
{
    HeartbeatCell[] heartbeats;
    //public void beat(int id)
    //{
    //    for(int i = 0; i < heartbeats.Length; i++)
    //    {
    //        heartbeats[i].initNonPlayer(i == id);
    //    }
    //}

    public override void realFinishedGame()
    {
        base.realFinishedGame();
        GetComponent<RhythmGameManager>().resetGame();
    }

    public override void startMinigame()
    {
        base.startMinigame();
        GetComponent<RhythmGameManager>(). startGame();
    }

    public void updateBeat(int currentBeat, Dictionary<int,int> beats)
    {
        if (!shouldUpdate())
        {
            return;
        }
        int j = 0;
        //Debug.Log("update beat " + currentBeat);
        for (int i = 0; i < heartbeats.Length; i++)
        {
            if (beats.ContainsKey(i))
            {

                heartbeats[i].initPlayer(i == currentBeat, beats[i]);

            }
            else
            {
                heartbeats[i].initNonPlayer(i == currentBeat);
            }
        }
    }

    public void addScore()
    {
        if (!shouldUpdate())
        {
            return;
        }
        progress += progressAdd;
        updateProgress();
    }

    public void reduceScore()
    {
        if (!shouldUpdate())
        {
            return;
        }
        progress -= progressReduce;
        updateProgress();
    }

    //public void playerBeat(bool succeed,int id)
    //{

    //    for (int i = 0; i < heartbeats.Length; i++)
    //    {
    //        heartbeats[i].initPlayer(i == id && succeed);
    //    }
    //}
    // Start is called before the first frame update
    void Start()
    {
        heartbeats = GetComponentsInChildren<HeartbeatCell>();
        startString = "Start smooth heart beat, press "+ keyArrow+" when it hit the heart to smooth";
        endString = "Heartbeat smooth succeed!";
        gameName = "Heartbeat";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
