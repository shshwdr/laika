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
    // Start is called before the first frame update
    void Start()
    {
       // showTextLabel("poemEnd3");
    }

    public bool showTextLabel(string name)
    {
        textLabel.gameObject.SetActive(true);
        var text = Resources.Load<TextAsset>("poem/" + name);
        if (!text)
        {
            return false;
        }

        //fadeout?
        dialogueSprites.SetActive(false);
        textLabel.text = text.text;
        textLabel.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        StartCoroutine(calculateText());
        return true;

    }

    IEnumerator calculateText()
    {
        yield return new WaitForSeconds(0.1f);
        var height = textLabel.rectTransform.rect.height;
        var finalHeight = 326/2 + height;
        
        var time = moveSpeed * finalHeight;
        var x = textLabel.rectTransform.position.x; 
        DOTween.To(() => textLabel.rectTransform.anchoredPosition, x => textLabel.rectTransform.anchoredPosition = x, new Vector2(0, finalHeight), moveSpeed* finalHeight);
        yield return new WaitForSeconds(time-2);
        EventPool.Trigger("finishedMovieText");

        dialogueSprites.SetActive(true);

        textLabel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
