using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartAndEndLogic : Singleton<StartAndEndLogic>
{

    public TMP_Text startText;
    public Image startPanel;
    public float startShowTime = 2;
    public float startHideTime = 2;

    public float endScrollSpeed = 0.04f;
    public TMP_Text endText;
    public Image endPanel;
    public Image endButton;

    public float endWaitTime = 10f;

    // public Camera mainCam;
    // public Camera startEndCam;
    public GameObject minigameManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startTextMoving());
        //StartCoroutine(endTextMoving());
    }

    public void end()
    {
        StartCoroutine(endTextMoving());
    }

    IEnumerator startTextMoving()
    {
        //Time.timeScale = 0;
        var text = Resources.Load<TextAsset>("poem/start");
        if (!text)
        {
            Debug.LogError("no start text");
        }
        startText.text = text.text;
        startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, 0);
        DOTween.To(() => startText.color, x => startText.color = x, new Color(startText.color.r, startText.color.g, startText.color.b,1), startShowTime).SetUpdate(true);
        yield return new WaitForSeconds(startShowTime+1);
        DOTween.To(() => startText.color, x => startText.color = x, new Color(startText.color.r, startText.color.g, startText.color.b, 0), startHideTime).SetUpdate(true);
        DOTween.To(() => startPanel.color, x => startPanel.color = x, new Color(startPanel.color.r, startPanel.color.g, startPanel.color.b, 0), startHideTime).SetUpdate(true);

        yield return new WaitForSeconds(startHideTime);
        //Time.timeScale = 1;
        startPanel.gameObject.SetActive(false);
        //startEndCam.gameObject.SetActive(false);
        //mainCam.gameObject.SetActive(true);
        minigameManager.SetActive(true);
    }

    IEnumerator endTextMoving()
    {
        var text = Resources.Load<TextAsset>("poem/end");
        if (!text)
        {
            Debug.LogError("no start text");
        }
        endText.text = text.text;
        endPanel.gameObject.SetActive(true);
        endPanel.color = new Color(endPanel.color.r, endPanel.color.g, endPanel.color.b, 0);
        DOTween.To(() => endPanel.color, x => endPanel.color = x, new Color(endPanel.color.r, endPanel.color.g, endPanel.color.b, 1), startHideTime).SetUpdate(true);
        //yield return new WaitForSeconds(startHideTime);
        var height = endText.rectTransform.rect.height;
        DOTween.To(() => endText.rectTransform.anchoredPosition, x => endText.rectTransform.anchoredPosition = x, new Vector2(0, height), endScrollSpeed*height).SetEase(Ease.Linear);
        yield return new WaitForSeconds(endWaitTime);
        endButton.gameObject.SetActive(true);
    }

    public void restartGame()
    {
        //Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
