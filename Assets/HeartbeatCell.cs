using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartbeatCell : MonoBehaviour
{
    public Image lineImage;
    public Image heartImage;
    public Image wrongImage;
    public Sprite flat;
    public Sprite spike;

    public Sprite heartImageGood;
    public Sprite heartImageBad;

    // Start is called before the first frame update
    void Start()
    {
    }


    public void initNonPlayer(bool isSpike)
    {
        lineImage.sprite = isSpike ? spike : flat;
        heartImage.gameObject.SetActive(false);
        wrongImage.gameObject.SetActive(false);
    }

    public void initPlayer(bool isCurrent,int succeed)
    {
        Debug.Log("init player " + isCurrent + " " + succeed);
        lineImage.sprite = isCurrent ? spike : flat;
        heartImage.gameObject.SetActive(true);
        wrongImage.gameObject.SetActive(false);
        if (succeed == 0)
        {
            heartImage.sprite = heartImageBad;
        }else if(succeed == 1)
        {

            heartImage.sprite = heartImageGood;
        }
        else
        {
            heartImage.sprite = heartImageBad;
            wrongImage.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
