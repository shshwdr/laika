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

    public Transform startTransform;

    public float endWaitTime = 10f;

    // public Camera mainCam;
    // public Camera startEndCam;
    public GameObject minigameManager;

    public float fadeInAndOut = 0.5f;

    List<Transform> dialogPanels = new List<Transform>();

    public GameObject mainCamera;

    IEnumerator WaitForInstruction()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                yield break;
            }
            yield return null;
        }
        //not here yield return null;
    }
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

    void changeImageColor(Image image, bool isShow)
    {
        if (!image)
        {
            return;
        }
        Color imageColor = image.color;
        var startColor = new Color(imageColor.r, imageColor.g, imageColor.b, 0);
        var endColor = new Color(imageColor.r, imageColor.g, imageColor.b, 1);
        if (!isShow)
        {
            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
        }
        image.color = startColor;
        DOTween.To(() => image.color, x => image.color = x, endColor, fadeInAndOut).SetUpdate(true);
    }
    void changeTextColor(TMP_Text image, bool isShow)
    {
        if (!image)
        {
            return;
        }
        Color imageColor = image.color;
        var startColor = new Color(imageColor.r, imageColor.g, imageColor.b, 0);
        var endColor = new Color(imageColor.r, imageColor.g, imageColor.b, 1);
        if (!isShow)
        {
            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
        }
        image.color = startColor;
        DOTween.To(() => image.color, x => image.color = x, endColor, fadeInAndOut).SetUpdate(true);
    }

    IEnumerator startTextMoving()
    {
        foreach (Transform dialogPanel in startTransform)
        {
            dialogPanels.Add(dialogPanel);
            dialogPanel.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0);

        List<Transform> dialogues = new List<Transform>();

        foreach(Transform panel in dialogPanels)
        {
            panel.gameObject.SetActive(true);
            dialogues.Clear();
            foreach (Transform dialog in panel)
            {
                dialogues.Add(dialog);
                dialog.gameObject.SetActive(false);
            }

            changeImageColor(panel.GetComponent<Image>(), true);
            yield return new WaitForSeconds(fadeInAndOut);
            yield return WaitForInstruction();





            foreach (Transform dialog in dialogues)
            {

                dialog.gameObject.SetActive(true);




                changeImageColor(dialog.GetComponent<Image>(), true);
                //changeTextColor(panel.GetComponent<TMP_Text>(), true);
                changeTextColor(dialog.GetComponentInChildren<TMP_Text>(), true);
                yield return new WaitForSeconds(fadeInAndOut);
                yield return WaitForInstruction();

                changeImageColor(dialog.GetComponent<Image>(), false);
                //changeTextColor(panel.GetComponent<TMP_Text>(), true);
                changeTextColor(dialog.GetComponentInChildren<TMP_Text>(), false);
                yield return new WaitForSeconds(fadeInAndOut);

                dialog.gameObject.SetActive(false);



                if (panel.name == "StartPanel")
                {
                    mainCamera.SetActive(true);
                }
            }
            changeImageColor(panel.GetComponent<Image>(), false);
            yield return new WaitForSeconds(fadeInAndOut);
            panel.gameObject.SetActive(false);
        }

        minigameManager.SetActive(true);


        //Time.timeScale = 0;
        //var text = Resources.Load<TextAsset>("poem/start");
        //if (!text)
        //{
        //    Debug.LogError("no start text");
        //}
        //startText.text = text.text;
        //startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, 0);
        //DOTween.To(() => startText.color, x => startText.color = x, new Color(startText.color.r, startText.color.g, startText.color.b,1), startShowTime).SetUpdate(true);
        //yield return new WaitForSeconds(startShowTime+1);
        //DOTween.To(() => startText.color, x => startText.color = x, new Color(startText.color.r, startText.color.g, startText.color.b, 0), startHideTime).SetUpdate(true);
        //DOTween.To(() => startPanel.color, x => startPanel.color = x, new Color(startPanel.color.r, startPanel.color.g, startPanel.color.b, 0), startHideTime).SetUpdate(true);

        //yield return new WaitForSeconds(startHideTime);
        ////Time.timeScale = 1;
        //startPanel.gameObject.SetActive(false);
        ////startEndCam.gameObject.SetActive(false);
        ////mainCam.gameObject.SetActive(true);
        //minigameManager.SetActive(true);
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
