using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pool;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameManager : Singleton<RhythmGameManager>
{
    bool shouldGetMoveInput = false;

    HeartbeatMiniGame minigame;

    public AudioClip masterBeat;
    public AudioClip commandMutedBeat;
    public AudioClip beatMissSigh;
    public AudioClip beatSkipSigh;

    AudioSource audioSourceSFX;

    public AudioSource audioSourceBeat;
    //AudioSource[] audioSources;

    float startTime;

    public AudioClip[] mutedBeatClips;
    int[] mutedBeats = new int[4];
    int mutedBeatId;

    bool allowedToBeat;
    //beat track variables
    [Header("Beat timing variables")]
    [Range(0, 150)]
    public float beatsPerMinute = 80;
    [Range(0, 1)]
    public float errorMarginTime = .3f;

    int beatPerRound = 8;
    int allowCurrentBeat = -1;


    Dictionary<int,int> playerInputBeats = new Dictionary<int, int> {};

    int lastBeat = 0;
    int currentBeat = 0;

    int commandCount = 0;
   // int inactiveBeatCount = 0;  //how many beats after command are inactive

    //measure how long an active beat time has no input
    float beatFallTime;

    //count how long beat is active without an input
    private float beatActiveTime = 0f;
    new private bool enabled;
    public float invokeTime;

    public float gridSize = 0.75f;

    private void Awake()
    {
        minigame = GetComponent<HeartbeatMiniGame>();
        //errorMarginTime = 
    }
    public void startMoveInput()
    {
        shouldGetMoveInput = true;
    }
    public void stopMoveInput()
    {
        shouldGetMoveInput = false;
    }

    public bool hasBeatInput
    {
        get
        {
            return enabled;
        }
        set
        {
            enabled = value;
            if (!enabled)
                beatActiveTime = Time.time;
        }
    }

    public void gameOver()
    {
        nextPlayerMasterBeat = 0;
        nextAllowBeat = 0;
    }



    bool lastBeatHasInput = true;   //true means no, false means yes, used with offset along with hasBeatInput

    //ui flash colour variables
    Color flashColor = new Color(255f, 255f, 255f, 1);
    Image currentDrumSprite = null;
    [Range(0.25f, 0.5f)]
    public float spriteFlashTime = 0.5f;




    public float extraLayerVolume = 0.3f;
    public float baseLayerVolume = 0.3f;

    float nextPlayerMasterBeat;
    float nextAllowBeat;

    float originNextAllowBeat;
    float originNextPlayerMasterBeat;


    int totalBeatCountNextAllow;
    int totalBeatCountNextPlayer;

    void Start()
    {



    }

    public void startGame()
    {

        allowedToBeat = false;

        hasBeatInput = false;


        invokeTime = 60f / beatsPerMinute;
        // errorMarginTime = invokeTime * 2;

        audioSourceSFX = GetComponent<AudioSource>();

        beatFallTime = errorMarginTime;
        StartCoroutine(startBeat());
        minigame.updateBeat(-1, playerInputBeats);
    }

    IEnumerator startBeat()
    {
        yield return new WaitForSeconds(invokeTime*2);

        originNextAllowBeat = (float)AudioSettings.dspTime - errorMarginTime / 2;
        originNextPlayerMasterBeat = (float)AudioSettings.dspTime;
        nextPlayerMasterBeat = originNextPlayerMasterBeat;
        nextAllowBeat = originNextAllowBeat;
        Debug.Log("next allow beat "+ nextAllowBeat + " " + nextPlayerMasterBeat);
    }

    public void resetGame()
    {
        originNextAllowBeat = 0;
        totalBeatCountNextAllow = 0;
        totalBeatCountNextPlayer = 0;
    }

    void Update()
    {
        if (minigame.isFinished)
        {
            return;
        }
       // if (GameManager.Instance.isInGame)
        {
            double time = AudioSettings.dspTime;
            if(originNextAllowBeat == 0)
            {
                return;
            }
            //if (nextAllowBeat != 0 || nextPlayerMasterBeat != 0)
            {

                if (time - invokeTime > nextAllowBeat)
                {
                    // We are now approx. 1 second before the time at which the sound should play,
                    // so we will schedule it now in order for the system to have enough time
                    // to prepare the playback at the specified time. This may involve opening
                    // buffering a streamed file and should therefore take any worst-case delay into account.

                    //Debug.Log("Scheduled source " + flip + " to start at time " + nextEventTime);
                    AllowBeat();
                    // Place the next event 16 beats from here at a rate of 140 beats per minute
                    totalBeatCountNextAllow++;
                    nextAllowBeat = invokeTime* totalBeatCountNextAllow+originNextAllowBeat;

                    // Flip between two audio sources so that the loading process of one does not interfere with the one that's playing out
                    //flip = 1 - flip;
                }
                if (time - invokeTime > nextPlayerMasterBeat)
                {
                    // We are now approx. 1 second before the time at which the sound should play,
                    // so we will schedule it now in order for the system to have enough time
                    // to prepare the playback at the specified time. This may involve opening
                    // buffering a streamed file and should therefore take any worst-case delay into account.
                    //audioSources[flip].clip = clips[flip];
                    //audioSourceBeat.PlayScheduled(nextPlayerMasterBeat);

                    // Place the next event 16 beats from here at a rate of 140 beats per minute
                    totalBeatCountNextPlayer++;
                    nextPlayerMasterBeat = invokeTime * totalBeatCountNextPlayer + originNextPlayerMasterBeat;
                    if(time - invokeTime > nextPlayerMasterBeat)
                    {

                    }
                    else
                    {

                        PlayMasterBeat();
                    }

                    // Flip between two audio sources so that the loading process of one does not interfere with the one that's playing out
                    //flip = 1 - flip;
                }
            }


        }


        beatFallTime -= Time.deltaTime;
        if (allowedToBeat && beatFallTime < 0f)
        {

            Debug.Log("cant beat now "+Time.deltaTime);
            allowedToBeat = false;

            if (playerInputBeats.ContainsKey(allowCurrentBeat))
            {
                if (playerInputBeats[allowCurrentBeat] == 0)
                {
                    audioSourceSFX.PlayOneShot(beatMissSigh);
                    playerInputBeats[allowCurrentBeat] = 2;

                    Debug.Log("miss beat " + currentBeat + " " + allowCurrentBeat);
                    minigame.reduceScore();
                }
            }
            //Debug.Log("not allow!");
        }

        if (allowedToBeat && hasBeatInput && Input.anyKeyDown)
        {      //double beat per master beat
           // Debug.Log("double beat not allowed");
            //hasBeatInput = false;
            lastBeatHasInput = true;
        }

        GetDrumInputs();


        if (!allowedToBeat && KeyBindingManager.GetKeyDown(KeyAction.up))
        {                     //mistiming beat with master beat
            if (minigame.shouldUpdate())
            {

                audioSourceSFX.PlayOneShot(beatMissSigh);
                minigame.reduceScore();
            }

            commandCount = 0;
        }



        //if (Time.time - beatActiveTime >= errorMarginTime && !lastBeatHasInput && allowedToBeat)
        //{      //skipping a master beat
        //    //Debug.Log("skipped");
        //    lastBeatHasInput = true;
        //   // lastBeat = currentBeat;
        //    if (playerInputBeats.ContainsKey(allowCurrentBeat))
        //    {
        //        if(playerInputBeats[allowCurrentBeat] == 0)
        //        {
        //            //audioSourceSFX.PlayOneShot(beatMissSigh);
        //               playerInputBeats[allowCurrentBeat] = 2;

        //            Debug.Log("miss beat " + currentBeat + " " + allowCurrentBeat);
        //            minigame.reduceScore();
        //        }
        //    }
        //}
    }


    void AllowBeat()
    {
        //Debug.Log("allow beat " + currentBeat + Time.deltaTime);
        if (playerInputBeats.ContainsKey(currentBeat))
        {
            allowCurrentBeat = currentBeat;

            beatFallTime = errorMarginTime;

            allowedToBeat = true;

            if (hasBeatInput)
            {
                hasBeatInput = false;
            }
        }
    }
    void PlayMasterBeat()
    {
        //Debug.Log("play master beat "+ Time.deltaTime);

        minigame.updateBeat(currentBeat, playerInputBeats);
        if (minigame.shouldUpdate())
        {
            audioSourceSFX.PlayOneShot(masterBeat);
        }

        //lastBeat = currentBeat;
        currentBeat++;
        if (currentBeat >= beatPerRound)
        {
            currentBeat = 0;

            playerInputBeats.Clear();
            if (minigame.shouldUpdate())
            {

                var rand1 = UnityEngine.Random.Range(4, 7);
                var rand2 = UnityEngine.Random.Range(4, 7);
                playerInputBeats[rand1] = 0;
                playerInputBeats[rand2] = 0;
            }

        }
        
    }

    void GetDrumInputs()
    {
        if (allowedToBeat && !hasBeatInput)
        {
            if (KeyBindingManager.GetKeyDown(KeyAction.up))
            {

                Debug.Log("beat " + currentBeat+" "+allowCurrentBeat);
                hasBeatInput = true;
                if (minigame.shouldUpdate())
                {
                    audioSourceSFX.PlayOneShot(commandMutedBeat);
                }
                if (playerInputBeats.ContainsKey(allowCurrentBeat))
                {
                    minigame.updateBeat(allowCurrentBeat, playerInputBeats);
                    playerInputBeats[allowCurrentBeat] = 1;
                    minigame.addScore();
                }
                else if (playerInputBeats.ContainsKey(allowCurrentBeat - 1))
                {

                    minigame.updateBeat(allowCurrentBeat - 1, playerInputBeats);
                    playerInputBeats[allowCurrentBeat - 1] = 1;
                    minigame.addScore();
                }
                else if (playerInputBeats.ContainsKey(allowCurrentBeat + 1))
                {

                    minigame.updateBeat(allowCurrentBeat + 1, playerInputBeats);
                    playerInputBeats[allowCurrentBeat + 1] = 1;
                    minigame.addScore();
                }
                else
                {
                    Debug.Log("I don't like this");
                }
            }
        }
        lastBeatHasInput = !hasBeatInput;
    }
}