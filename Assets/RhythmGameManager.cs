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


    Dictionary<int,int> playerInputBeats = new Dictionary<int, int> {};

    int lastBeat = 0;
    int currentBeat = 0;


    int[] commandType;
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

    void Start()
    {


        allowedToBeat = true;
        hasBeatInput = false;

        //inactiveBeatCount = 0;

        invokeTime = 60f / beatsPerMinute;

        commandType = new int[4] { 0, 0, 0, 0 };
        audioSourceSFX = GetComponent<AudioSource>();

        beatFallTime = errorMarginTime;
        StartCoroutine(startBeat2());
        invokeTime = 60f / beatsPerMinute;
        minigame.updateBeat(-1, playerInputBeats);


    }

    IEnumerator startBeat2()
    {
        yield return new WaitForSeconds(0.5f);

        nextAllowBeat = (float)AudioSettings.dspTime - errorMarginTime / 2;
        nextPlayerMasterBeat = (float)AudioSettings.dspTime;
    }

    void Update()
    {
       // if (GameManager.Instance.isInGame)
        {
            double time = AudioSettings.dspTime;
            if (nextAllowBeat != 0 || nextPlayerMasterBeat != 0)
            {

                if (time - 1.0f > nextAllowBeat)
                {
                    // We are now approx. 1 second before the time at which the sound should play,
                    // so we will schedule it now in order for the system to have enough time
                    // to prepare the playback at the specified time. This may involve opening
                    // buffering a streamed file and should therefore take any worst-case delay into account.
                    //audioSources[flip].clip = clips[flip];
                    //audioSources[flip].PlayScheduled(nextEventTime);

                    //Debug.Log("Scheduled source " + flip + " to start at time " + nextEventTime);
                    AllowBeat();
                    // Place the next event 16 beats from here at a rate of 140 beats per minute
                    nextAllowBeat += invokeTime;

                    // Flip between two audio sources so that the loading process of one does not interfere with the one that's playing out
                    //flip = 1 - flip;
                }
                if (time - 1.0f > nextPlayerMasterBeat)
                {
                    // We are now approx. 1 second before the time at which the sound should play,
                    // so we will schedule it now in order for the system to have enough time
                    // to prepare the playback at the specified time. This may involve opening
                    // buffering a streamed file and should therefore take any worst-case delay into account.
                    //audioSources[flip].clip = clips[flip];
                    //audioSourceBeat.PlayScheduled(nextPlayerMasterBeat);

                    //Debug.Log("Scheduled source " + flip + " to start at time " + nextEventTime);
                    PlayMasterBeat();
                    // Place the next event 16 beats from here at a rate of 140 beats per minute
                    nextPlayerMasterBeat += invokeTime;

                    // Flip between two audio sources so that the loading process of one does not interfere with the one that's playing out
                    //flip = 1 - flip;
                }
            }


            //if (audioSources[1].clip.length - audioSources[1].time < 1)
            //{
            //    audioSources[1].time = 0;
            //    audioSources[1].Play();
            //    for (int i = 1; i < audioSources.Length; i++)
            //    {
            //        if (audioSources[i].isPlaying)
            //        {

            //            audioSources[i].time = 0;
            //            audioSources[i].Play();
            //        }
            //    }
            //}
        }

        //if (lastAllowBeat - audioSources[1].time >10)
        //{
        //    lastPlayerMasterBeat = errorMarginTime / 2f;
        //    lastAllowBeat = 0;
        //}
        //    if (audioSources[1].time - lastAllowBeat >= invokeTime-0.05f)
        //{
        //    AllowBeat();
        //    lastAllowBeat += invokeTime;
        //}
        //if (audioSources[1].time - lastPlayerMasterBeat >= invokeTime - 0.05f)
        //{
        //    PlayMasterBeat();
        //    lastPlayerMasterBeat += invokeTime;
        //}





        beatFallTime -= Time.deltaTime;
        if (beatFallTime < 0f)
        {

            //Debug.Log("cant beat now");
            allowedToBeat = false;
            //Debug.Log("not allow!");
            //if (commandType[3] != 0)
            //{
            //    bool commandMatched = SetInput(commandType);
            //    if (commandMatched)
            //    {
            //        commandCount++;
            //        inactiveBeatCount = 4;      //4 beats after input are inactive
            //    }
            //    else
            //    {
            //        inactiveBeatCount = 0;
            //        commandCount = 0;
            //    }
            //    clearCommand();
            //}
        }

        if (allowedToBeat && hasBeatInput && Input.anyKeyDown)
        {      //double beat per master beat
           // Debug.Log("double beat not allowed");
            //hasBeatInput = false;
            lastBeatHasInput = true;
            clearCommand();
        }

        GetDrumInputs();


        if (!allowedToBeat && Input.GetKeyDown(KeyCode.Space))
        {                     //mistiming beat with master beat
            if (minigame.shouldUpdate())
            {

                audioSourceSFX.PlayOneShot(beatMissSigh);
            }

            //Debug.Log("mistiming");
            clearCommand();
            commandCount = 0;
        }

        //if (inactiveBeatCount > 0 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        //{               //interrupting command
        //    clearCommand();
        //    commandCount = 0;
        //    //do physical motion stop here
        //}


        if (Time.time - beatActiveTime >= errorMarginTime && lastBeatHasInput && allowedToBeat)
        {      //skipping a master beat
            //Debug.Log("skipped");
            lastBeatHasInput = true;
           // lastBeat = currentBeat;
            if (playerInputBeats.ContainsKey(currentBeat))
            {
                if(playerInputBeats[currentBeat] == 0)
                {
                    //audioSourceSFX.PlayOneShot(beatMissSigh);
                      playerInputBeats[currentBeat] = 2;
                    minigame.reduceScore();
                }
            }
            clearCommand();
            //addMoveInput(-1);
            //clearMoveInput();
        }

        //if (currentDrumSprite != null)
        //{
        //    Image temporaryReference = currentDrumSprite;
        //    temporaryReference.color = Color.Lerp(currentDrumSprite.color, Color.clear, spriteFlashTime);
        //}


        //continuos beats required to maintain fever
        //if (commandCount >= 4)
        //{
        //    fever = true;
        //    feverSprite.gameObject.SetActive(true);
        //}

        //if (inactiveBeatCount >= 0)
        //{
        //    feverTimeHold = Time.time;
        //}
        //if (Time.time - feverTimeHold >= ((errorMarginTime) * 2) + 1f && fever)
        //{
        //    commandCount = 0;
        //    fever = false;
        //    feverSprite.gameObject.SetActive(false);
        //}
    }

    void clearCommand()
    {

        Array.Clear(commandType, 0, commandType.Length);
        EventPool.Trigger("BeatClear");
    }
    int allowCurrentBeat = -1;

    void AllowBeat()
    {
        allowCurrentBeat++;
        if (allowCurrentBeat >= beatPerRound)
        {
            allowCurrentBeat = 0;
        }
        if (playerInputBeats.ContainsKey(currentBeat))
        {
            beatFallTime = errorMarginTime;

            //if (inactiveBeatCount == 0)
            //    teamController.resetSpritesToIdle();

            //Debug.Log("CAN BEAT");
            allowedToBeat = true;
            //Debug.Log("allow!");
            //inactiveBeatCount--;
            if (hasBeatInput)
            {
                hasBeatInput = false;
            }
        }
    }
    void PlayMasterBeat()
    {



        if (playerInputBeats.ContainsKey(currentBeat))
        {
            //Debug.Log("can beat now");
            //use map to find beat
            //audioSourceSFX.PlayOneShot(commandMutedBeat);
            //audioSourceSFX.PlayOneShot(mutedBeatClips[mutedBeats[mutedBeatId] - 1]);
            // mutedBeatId++;
        }
        else
        {

            minigame.updateBeat(currentBeat, playerInputBeats);
            if (minigame.shouldUpdate())
            {
                audioSourceSFX.PlayOneShot(masterBeat);
            }
        }
        lastBeat = currentBeat;
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
        //Debug.Log("dps time "+ AudioSettings.dspTime+" beat time " + (Time.time - startTime) + " music time " + audioSources[1].time+" diff " + (Time.time - startTime - audioSources[1].time));
        //player.Move();
        //EventPool.Trigger("Beat");
        
    }

    //bool SetInput(int[] commandType)
    //{
    //    bool commandMatched = false;// teamController.GetInput(commandType, ref mutedBeats);
    //    mutedBeatId = 0;
    //    return commandMatched;
    //}
    //public List<int> moveInput = new List<int>();
    //void addMoveInput(int i)
    //{
    //    if (shouldGetMoveInput)
    //    {

    //        moveInput.Add(i);
    //        EventPool.Trigger("player move input");
    //    }
    //}
    //public void clearMoveInput()
    //{
    //    moveInput.Clear();
    //}

    void GetDrumInputs()
    {
        //if (player.isConversation || !GameManager.Instance.isInGame || player.isDead)
        //{
        //    return;
        //}

        if (allowedToBeat && !hasBeatInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                hasBeatInput = true;
                Debug.Log("beat!");
                if (playerInputBeats.ContainsKey(allowCurrentBeat))
                {
                    playerInputBeats[allowCurrentBeat] = 1;
                    minigame.addScore();
                }
                minigame.updateBeat(allowCurrentBeat, playerInputBeats);
                if (minigame.shouldUpdate())
                {
                    audioSourceSFX.PlayOneShot(commandMutedBeat);
                }
            }
            //if (!Input.GetKey(KeyCode.Space) && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
            //{
            //    clearCommand();
            //    commandCount = 0;
            //}
        }
        lastBeatHasInput = !hasBeatInput;
    }
}