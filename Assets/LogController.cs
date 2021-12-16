using DG.Tweening;
using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionLog {
    public string idea;
    public int success;
    public List<string> log;
}



public class LogController : Singleton<LogController>
{
    public Transform content;
    ScrollRect scrollRect;
    static float originX = 10;
    public Dictionary<string, Dictionary<int,List<string>>> actionBubbleInfoDict = new Dictionary<string, Dictionary<int, List<string>>>();

    private void Awake()
    {
        //var logs = CsvUtil.LoadObjects<ActionLog>("Log");
        //foreach (var info in logs)
        //{
        //    if (!actionBubbleInfoDict.ContainsKey(info.idea)){

        //        actionBubbleInfoDict[info.idea] = new Dictionary<int, List<string>>();
        //    }

        //    actionBubbleInfoDict[info.idea][info.success] = info.log;
        //}
    }

    //public string getActionLog(string actionName, int success)
    //{
    //    if (!actionBubbleInfoDict.ContainsKey(actionName))
    //    {
    //        Debug.LogError("action name does not exist in log " + actionName);
    //        return "";
    //    }
    //    if (!actionBubbleInfoDict[actionName].ContainsKey(success))
    //    {
    //        Debug.LogError("action success does not exist in log " + actionName+" "+success);
    //        return "";
    //    }
    //    return Utils.randomFromList( actionBubbleInfoDict[actionName][success]);
    //}

    public static void ScrollToTop(ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, 1);
    }
    public static void ScrollToBottom(ScrollRect scrollRect)
    {
        DOTween.To(() => scrollRect.normalizedPosition, x => scrollRect.normalizedPosition = x, new Vector2(scrollRect.normalizedPosition.x, 0), 0.5f);

        //scrollRect.normalizedPosition = new Vector2(originX, 0);
    }

    public void addLog(string str, bool immediate = false)
    {
        addLog(str, Color.white,immediate);
    }
    public void addLog(string str,Color color,bool immediate = false)
    {
        foreach(Transform tran in content)
        {
            tran.GetComponent<LogPanel>().updateColor(Color.white);
        }


        var logPrefab = Resources.Load<GameObject>("log");
        var go = Instantiate(logPrefab);
        go.GetComponent<LogPanel>().init(str,Color.yellow);
        go.transform.SetParent(content,true);
        go.GetComponent<RectTransform>() .localScale = Vector3.one;

        if (!immediate)
        {
            StartCoroutine(test(go));
        }
        else
        {
            updateUI(go);
        }
    }

    IEnumerator test(GameObject go)
    {
        yield return new WaitForSeconds(0.01f);
        updateUI(go);
    }

    void updateUI(GameObject go)
    {
        go.SetActive(false);
        go.SetActive(true);

        //yield return new WaitForSeconds(0.1f);
        // if (scrollRect.GetComponent<RectTransform>().rect.height <= content.GetComponent<RectTransform>().rect.height)
        {
            ScrollToBottom(scrollRect);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
