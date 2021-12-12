using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    public TMP_Text logLabel;
    public Image panelImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void init(string str, Color color)
    {
        logLabel.text = str;
        logLabel.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
