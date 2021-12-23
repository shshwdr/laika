using DG.Tweening;
using Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovieTextController : Singleton<MovieTextController>
{
    public TMP_Text textLabel;
    public float moveSpeed = 0.1f;
    public GameObject dialogueSprites;

    bool canSkip = false;
    public GameObject skipText;
    bool isEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        skipText.SetActive(false);
        // showTextLabel("poemEnd1");
        //StartCoroutine(randomTextState());
        //StartCoroutine(randomTextState2());
        //StartCoroutine(randomTextState3());
    }

    public bool showTextLabel(string name)
    {
        var text = Resources.Load<TextAsset>("poem/" + name);
        if (!text)
        {
            return false;
        }
        canSkip = false;
        isEnded = false;
        textLabel.gameObject.SetActive(true);
        //fadeout?
        dialogueSprites.SetActive(false);
        textLabel.text = text.text;
        textLabel.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        StartCoroutine(calculateText());
        return true;

    }

    IEnumerator randomTextState()
    {
        var material = textLabel.fontSharedMaterial;
        for(int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
            material.SetFloat("_GlowOffset", Random.Range(0f, 1f));
        }
        //DOTween.To(() => textLabel.rectTransform.anchoredPosition, x => textLabel.rectTransform.anchoredPosition = x, new Vector2(0, finalHeight), moveSpeed * finalHeight);
    }

    IEnumerator randomTextState2()
    {
        var material = textLabel.fontSharedMaterial;
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
            material.SetFloat("_UnderlayDilate", Random.Range(0f, 1f));
        }
        //DOTween.To(() => textLabel.rectTransform.anchoredPosition, x => textLabel.rectTransform.anchoredPosition = x, new Vector2(0, finalHeight), moveSpeed * finalHeight);
    }

    IEnumerator randomTextState3()
    {
        var material = textLabel.fontSharedMaterial;
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
            material.SetFloat("_OutlineThickness", Random.Range(0f, 1f));
        }
        //DOTween.To(() => textLabel.rectTransform.anchoredPosition, x => textLabel.rectTransform.anchoredPosition = x, new Vector2(0, finalHeight), moveSpeed * finalHeight);
    }

    IEnumerator calculateText()
    {
        yield return new WaitForSeconds(0.1f);
        var height = textLabel.rectTransform.rect.height;
        var finalHeight = 326 + height;
        
        var time = moveSpeed * finalHeight;
        var time2 = moveSpeed * (height +50);
        var x = textLabel.rectTransform.position.x; 
        DOTween.To(() => textLabel.rectTransform.anchoredPosition, x => textLabel.rectTransform.anchoredPosition = x, new Vector2(0, finalHeight), time).SetEase(Ease.Linear);

        yield return new WaitForSeconds(time2);

        canSkip = true;

        skipText.SetActive(true);

        yield return new WaitForSeconds(time-time2);




        //yield return new WaitForSeconds(2);
        if(!isEnded)
        finish();
    }

    void finish()
    {
        EventPool.Trigger("finishedMovieText");
        skipText.SetActive(false);
        canSkip = false;
        dialogueSprites.SetActive(true);

        textLabel.gameObject.SetActive(false);
        isEnded = true;
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSkip)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                finish();
            }
        }

        if (CheatManager.Instance.canCheat)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                finish();
            }
        }
    }
}
