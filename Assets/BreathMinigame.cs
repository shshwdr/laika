using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreathMinigame : MiniGame
{
    [SerializeField] protected float progressReduceNatural = 0.05f;

    //public Sprite inRangeHook;
    //public Sprite outRangeHook;

    public Image fish;
    public Image hook;


    float fishDestination;
    float fishTimer;
    float fishSpeed;
    float fishPosition;

    [SerializeField] float timerMultiplicator = 3;
    [SerializeField] float smoothMotion = 1;


    [SerializeField] float hookSize = 30f;
    float hookPosition;
    float hookPullVelocity;
    [SerializeField]
    float hookPullVelocityMax = 1f;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.002f;

    float currentRoundWaitTime = 0;
    [SerializeField] float roundWaitTime = 1f;
    bool isRoundFinished = false;

    public Image correctImage;
    public Image wrongImage;

    public AudioClip correct;
    public AudioClip wrong;

    // Start is called before the first frame update
    void Start()
    {
        startString = "Start indicate when to breath, press "+keyArrow+" to indicate";
        endString = "Indicate breath succeed!";
        gameName = "Breath";
        resizeHook();
        fishDestination = 45;
        //startMinigame();
        correctImage.gameObject.SetActive(false);
        wrongImage.gameObject.SetActive(false);
        moveHook();
    }

    void resizeHook()
    {
        hook.rectTransform.sizeDelta = new Vector2(hook.rectTransform.sizeDelta.x, hookSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished)
        {
            return;
        }
        if (isRoundFinished)
        {
            currentRoundWaitTime += Time.deltaTime;
            if (currentRoundWaitTime >= roundWaitTime)
            {
                isRoundFinished = false;
                currentRoundWaitTime = 0;
                fishTimer = 0;
                correctImage.gameObject.SetActive(false);
                wrongImage.gameObject.SetActive(false);
                moveHook();
            }
            return;
        }

        //fishTimer -= Time.deltaTime;
        if (fishDestination == -50 && fishPosition <= fishDestination+1)
        {
            fishTimer = timerMultiplicator;
            fishDestination =45;


            progress -= progressReduceNatural;
            updateProgress();
        }
        if (fishDestination == 45 && fishPosition >= fishDestination-1)
        {
            fishTimer = timerMultiplicator;
            fishDestination = -50;


            progress -= progressReduceNatural;
            updateProgress();
        }

        moveFish();
        if (KeyBindingManager.GetKeyDown(KeyAction.right))
        {
            progressCheck();
        }
    }

    public IEnumerator waitAndRestart()
    {
        yield return new WaitForSeconds(5f);


        progressAdd -= 0.05f;
        progressAdd = Mathf.Max(progressAdd, progressReduce * 2);
        hookSize -= 5;
        hookSize = Mathf.Max(hookSize, 20);

        resizeHook();

        startMinigame();

    }

    public override void startMinigame()
    {
        base.startMinigame();


        correctImage.gameObject.SetActive(false);
        wrongImage.gameObject.SetActive(false);
        moveHook();
        fishPosition = 0;

    }


    void progressCheck()
    {
        float hookMin = hookPosition - hookSize / 2;
        float hookMax = hookPosition + hookSize / 2;
        if (hookMin <= fishPosition && hookMax >= fishPosition)
        {
            progress += progressAdd;
            correctImage.gameObject.SetActive(true);
            sfx.PlayOneShot(correct);
        }
        else
        {
            progress -= progressReduce;
            wrongImage.gameObject.SetActive(true);
            sfx.PlayOneShot(wrong);
        }
        isRoundFinished = true;
        updateProgress();
    }
    void moveHook()
    {

        hookPosition = (Random.value -0.5f) *2* (50 - hookSize/2);

        hook.rectTransform.localPosition = new Vector3(0, hookPosition, 0); ;

    }

    void moveFish()
    {
        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.rectTransform.localPosition = new Vector3(0, fishPosition);
    }
}
