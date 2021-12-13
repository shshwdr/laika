using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TemporatureMiniGame : MiniGame
{
    public Image meter;

    public Sprite inRangeHook;
    public Sprite outRangeHook;

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
    [SerializeField] float hookGravityPower = 0.005f;




    // Start is called before the first frame update
    void Start()
    {
        startString = "Start adjust temporature, press Left Arrow to adjust";
        endString = "Adjust temporature succeed!";
        //startMinigame();
        resizeHook();
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
        moveFish();
        moveHook();
        progressCheck();
    }

    public IEnumerator waitAndRestart()
    {
        yield return new WaitForSeconds(5f);


        progressAdd -= 0.05f;
        progressAdd = Mathf.Max(progressAdd, progressReduce* 2);
        hookSize -= 5;
        hookSize = Mathf.Max(hookSize, 20);

        resizeHook();

        startMinigame();

    }


    void progressCheck()
    {
        float hookMin = hookPosition - hookSize/2;
        float hookMax = hookPosition + hookSize/2;
        if(hookMin<=fishPosition && hookMax >= fishPosition)
        {
            progress += progressAdd * Time.deltaTime;
            hook.sprite = inRangeHook;
        }
        else
        {
            progress -= progressReduce * Time.deltaTime;
            hook.sprite = outRangeHook;
        }
        updateProgress();
    }
    void moveHook()
    {
        float hookMin = -(50 - hookSize / 2);
        float hookMax = 50 - hookSize / 2;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            hookPullVelocity += hookPullPower * Time.deltaTime;
        }else if (Mathf.Approximately(hookPosition, hookMin) || Mathf.Approximately(hookPosition, hookMax))
        {
            hookPullVelocity /= 2;
        }

        hookPullVelocity -= hookGravityPower * Time.deltaTime;
        hookPullVelocity = Mathf.Clamp(hookPullVelocity, -hookPullVelocityMax, hookPullVelocityMax);
        hookPosition += hookPullVelocity;
        hookPosition = Mathf.Clamp(hookPosition ,- (50-hookSize / 2), 50-hookSize / 2);
        
        hook.rectTransform.localPosition =new  Vector3(0, hookPosition, 0); ;

    }

    void moveFish()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0)
        {
            fishTimer = Random.value * timerMultiplicator;
            fishDestination = Random.value * 95f - 50;
        }
        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.rectTransform.localPosition = new Vector3(0, fishPosition);
    }
}
